using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TheOneCRM.Application.Interfaces.ICampaign;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;
using TheOneCRM.Infrastructure.Specsification.Customerspec;

namespace TheOneCRM.Application.Services
{
    public class CampaignService : ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CampaignService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IReadOnlyList<CampaignDropdownDto>> GetCampaignsForDropdownAsync()
        {
            var spec = new ActiveCampaignsSpec();
            var campaigns = await _unitOfWork.Repository<Campaigns>().ListWithSelectAsync(spec, x => new CampaignDropdownDto { Id = x.Id, Name = x.Name });
            return campaigns;
            //return _mapper.Map<IReadOnlyList<CampaignDropdownDto>>(campaigns);
        }

        public async Task<CampaignResponseDto> CreateCampaignAsync(CreateCampaignDto dto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedAccessException("User not found");

            var isMarketing = await _userManager.IsInRoleAsync(user, "Marketing");
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isMarketing && !isAdmin)
                throw new UnauthorizedAccessException("Only Marketing and Admin users can create campaigns");
            // 1️⃣ Validate Channel Source
            var source = await _unitOfWork.Repository<ChannelSource>()
                .GetByIdAsync(dto.ChannelSourceId.Value);

            if (source == null)
                throw new KeyNotFoundException("Invalid Channel Source");

            // 2️⃣ Map main entity
            var campaign = _mapper.Map<Campaigns>(dto);
            campaign.AppUserId = userId;
            campaign.ChannelSourceId = (int)dto.ChannelSourceId;
            if (dto.Countries != null && dto.Countries.Any())
            {
                campaign.Countries = dto.Countries
                    .Select(c => new CampaignCountry
                    {
                        Id = c
                    })
                    .ToList();
            }
            // 3️⃣ Dates validation
            //if (dto.EndDate < dto.StartDate)
            //    throw new InvalidOperationException("EndDate must be after StartDate");

            // 4️⃣ Save
            await _unitOfWork.Repository<Campaigns>().AddAsync(campaign);
            await _unitOfWork.SaveChangesAsync();

            // 5️⃣ IMPORTANT: reload with specification (for response)
            var spec = new CampaignWithDetailsSpec(campaign.Id);

            var createdCampaign = await _unitOfWork.Repository<Campaigns>()
                .GetEntityWithSpec(spec);

            // 6️⃣ Map response
            return _mapper.Map<CampaignResponseDto>(createdCampaign);
        }
        public async Task<Pagination<CampaignListItemDto>> GetAllCampaignsAsync(
    CampaignPaginationParams paginationParams)
        {
            var spec = new CampaignsWithPaginationSpec(paginationParams);
            var countSpec = new CampaignsCountSpec(paginationParams);

            var campaigns = await _unitOfWork.Repository<Campaigns>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Campaigns>().CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<CampaignListItemDto>>(campaigns);

            return new Pagination<CampaignListItemDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                totalCount,
                data
            );
        }

        public async Task<CampaignResponseDto> ToggleCampaignStatusAsync(int id)
        {
            var campaign = await _unitOfWork.Repository<Campaigns>()
                .GetEntityWithSpec(new CampaignByIdSpec(id));

            if (campaign == null)
                throw new KeyNotFoundException($"Campaign {id} not found");

            if (campaign.Status == CampaignStatus.Completed ||
                campaign.Status == CampaignStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot toggle a completed or cancelled campaign");
            }

            campaign.Status = campaign.Status == CampaignStatus.Active
                ? CampaignStatus.NotActive
                : CampaignStatus.Active;

            _unitOfWork.Repository<Campaigns>().Update(campaign);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CampaignResponseDto>(campaign);
        }
        public async Task DeleteCampaignAsync(int id)
        {
            // 1) جيب الحملة
            var campaign = await _unitOfWork.Repository<Campaigns>()
                .GetEntityWithSpec(new CampaignByIdSpec(id));

            // 2) لو مش موجودة
            if (campaign == null)
                throw new KeyNotFoundException($"Campaign with id {id} not found");

            // 3) احذف الحملة
            _unitOfWork.Repository<Campaigns>().Delete(campaign);

            // 4) احفظ التغييرات
             await _unitOfWork.SaveChangesAsync();

          
        }
        public async Task<List<CampaignDashboardDto>> GetCampaignsDashboardAsync()
        {
            var campaigns = await _unitOfWork.Repository<Campaigns>()
                .ListAsync(new CampaignsSpec());

            // map لكل الحملات
            var dtos = _mapper.Map<List<CampaignDetailsDto>>(campaigns);

            // نحسب لكل واحدة
            foreach (var dto in dtos)
            {
                CalculateBudgetMetrics(dto);
            }

            // ===== التجميع =====
            var result = new CampaignDashboardDto
            {
                TotalCampaigns = dtos.Count,

                ActiveCampaigns = dtos.Count(c => c.Status == "Active"),

                TotalBudget = dtos.Sum(c => c.Budget),

                TotalSpent = dtos.Sum(c => c.Spent),

                TotalRemaining = dtos.Sum(c => c.Remaining)
            };

            return new List<CampaignDashboardDto> { result };
        }
        public async Task<CampaignDetailsDto> GetCampaignByIdAsync(int id)
        {
            // 1) جيب العميل مع البيانات المرتبطة
            var Campaigns = await _unitOfWork.Repository<Campaigns>()
                .GetEntityWithSpec(new CampaignByIdSpec(id));

            // 2) لو مش موجود
            if (Campaigns == null)
                throw new KeyNotFoundException($"Campaign with id {id} not found");
            var dto = _mapper.Map<CampaignDetailsDto>(Campaigns);

            CalculateBudgetMetrics(dto);

            return dto;
        
        }

        public async Task<List<CampaignPerformanceRowDto>> GetCampaignPerformance()
        {
            var campaigns = await _unitOfWork.Repository<Campaigns>().ListWithSelectAsync(spec: null,
        selector: c => new { c.Id, c.Name });

            var allBuyers = await _unitOfWork.Repository<Customer>()
                .ListAsync(new BuyerCustomersSpecification());
            var allNonBuyers = await _unitOfWork.Repository<Customer>()
                .ListAsync(new NotBuyerCustomersSpecification());

            var rows = campaigns.Select(c =>
            {
                var buyers = allBuyers.Count(x => x.compaignId == c.Id);
                var nonBuyers = allNonBuyers.Count(x => x.compaignId == c.Id);
                var total = buyers + nonBuyers;

                return new CampaignPerformanceRowDto
                {
                    CampaignId = c.Id,
                    CampaignName = c.Name, 

                    TotalCustomers = total,
                    Buyers = buyers,
                    NonBuyers = nonBuyers,
                    ConversionRate = total == 0
                        ? 0m
                        : Math.Round((decimal)buyers * 100 / total, 1)
                };
            })
            .OrderByDescending(x => x.TotalCustomers)
            .ToList();

       
            return rows;
        }



     
        private static void CalculateBudgetMetrics(CampaignDetailsDto dto)
        {
            // ===== 1) الميزانية اليومية =====
            // الميزانية اليومية = الميزانية الإجمالية ÷ عدد أيام الحملة
            dto.DailyBudget = dto.DurationDays > 0
                ? Math.Round(dto.Budget / dto.DurationDays, 2)
                : 0;

            // ===== 2) حساب عدد الأيام اللي عدّت من بداية الحملة =====
            var today = DateTime.UtcNow.Date;
            var startDate = dto.StartDate.Date;
            var endDate = dto.EndDate.Date;

            int daysElapsed;

            if (today < startDate)
            {
                // الحملة لسه ما بدأتش
                daysElapsed = 0;
            }
            else if (today >= endDate)
            {
                // الحملة خلصت → نعتبر إن كل الأيام عدّت
                daysElapsed = dto.DurationDays;
            }
            else
            {
                // الحملة شغالة دلوقتي → نحسب الفرق بين النهاردة وبداية الحملة
                daysElapsed = (today - startDate).Days + 1; // +1 عشان نحسب اليوم الحالي
                if (daysElapsed > dto.DurationDays)
                    daysElapsed = dto.DurationDays;
            }

            dto.DaysElapsed = daysElapsed;
            dto.DaysRemaining = Math.Max(0, dto.DurationDays - daysElapsed);

            // ===== 3) تم الإنفاق =====
            // تم الإنفاق = الميزانية اليومية × عدد الأيام اللي عدّت
            dto.Spent = Math.Round(dto.DailyBudget * daysElapsed, 2);

            // ضمان إن المنفق ما يتعداش الميزانية الإجمالية
            if (dto.Spent > dto.Budget)
                dto.Spent = dto.Budget;

            // ===== 4) المتبقي =====
            // المتبقي = الميزانية الإجمالية − تم الإنفاق
            dto.Remaining = dto.Budget - dto.Spent;

            // ===== 5) النسبة المئوية للإنفاق =====
            dto.SpentPercentage = dto.Budget > 0
                ? Math.Round((dto.Spent / dto.Budget) * 100, 1)
                : 0;
        }
    }
    }
