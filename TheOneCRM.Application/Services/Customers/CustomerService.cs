using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Application.Interfaces;
using TheOneCRM.Application.Interfaces.ICustomers;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.Constants;
using TheOneCRM.Domain.Models.DTOs;
using TheOneCRM.Domain.Models.DTOs.CampaignDto;
using TheOneCRM.Domain.Models.DTOs.Common;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Infrastructure.Migrations;
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;
using TheOneCRM.Infrastructure.Specsification.CustomerNotes;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
using static TheOneCRM.Infrastructure.Specsification.CustomersWithFilterSpec;

namespace TheOneCRM.Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto, string currentUserId, string currentUserRole)
        {

            if (dto.CampaignId > 0)
            {
                //var campaign = await _unitOfWork.Repository<Campaigns>()
                //    .GetEntityWithSpec(new CampaignByIdSpec((int)dto.CampaignId));
                var campaign = await _unitOfWork.Repository<Campaigns>()
                    .GetByIdAsync((int)dto.CampaignId);


                if (campaign == null)
                    throw new KeyNotFoundException(
                        $"Campaign with id {dto.CampaignId} not found");
            }
            var customer = _mapper.Map<Customer>(dto);
            customer.CreatedById = currentUserId;
            // 2) تحقق من الخدمات وضيفهم
            if (dto.ServiceIds != null && dto.ServiceIds.Any())
            {
                var existingServices = await _unitOfWork.Repository<Service>()
                    .ListAsync(new ServicesByIdsSpec(dto.ServiceIds));

                var validServiceIds = existingServices.Select(s => 
                
                
                
                
                s.Id).ToList();
                var missingIds = dto.ServiceIds.Except(validServiceIds).ToList();

                if (missingIds.Any())
                    throw new KeyNotFoundException(
                        $"Services not found: {string.Join(", ", missingIds)}");

                foreach (var serviceId in validServiceIds)
                {
                    customer.customerServices.Add(new CustomerServices
                    {
                        ServiceId = serviceId
                    });
                }
            }
            // 3) ✅ التعيين لو متطلوب
            if (dto.AssignToSalesTeam)
            {
                if (string.IsNullOrWhiteSpace(dto.SalesPersonId))
                    throw new InvalidOperationException("SalesPersonId is required when assigning to sales team");


                var salesPerson = await _userManager.FindByIdAsync(dto.SalesPersonId);
                if (salesPerson == null)
                    throw new KeyNotFoundException($"User with id {dto.SalesPersonId} not found");

                var isSalesPerson = await _userManager.IsInRoleAsync(salesPerson, "Sales");
                if (!isSalesPerson)
                    throw new InvalidOperationException(
                        $"User '{salesPerson.UserName}' is not a sales person and cannot be assigned customers");

                // ✅ سجل في الـ History
                customer.AssignmentHistory.Add(new CustomerAssignmentHistory
                {
                    FromUserId = currentUserId,
                    FromRole = currentUserRole,
                    ToUserId = dto.SalesPersonId,
                    ToRole = UserRoles.Sales,

                    AssignedAt = DateTime.UtcNow
                });
                // ✅ سجل في الـ History
                //var history = new CustomerAssignmentHistory
                //{
                //    FromUserId = currentUserId,
                //    FromRole = currentUserRole,
                //    ToUserId = dto.SalesPersonId,
                //    ToRole = UserRoles.Sales,
                //    AssignedAt = DateTime.UtcNow
                //};

                customer.IsMarketingToSales=true;   // ✅ بيحدد IsMarketingToSales تلقائياً

                //customer.AssignmentHistory.Add(history);
                customer.AssignedToId = dto.SalesPersonId;
                customer.CreatedById = currentUserId;
                customer.status = StatusOfCustomers.AssignedToSalesTeam;


            }
            // 4) ✅ ضيف الملاحظة الأولية لو موجودة
            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                customer.Notes.Add(new CustomerNote
                {
                    NoteMarketing = dto.Notes,
                    MarketingCreatedById = currentUserId,
                    MarketingCreatedAt = DateTime.UtcNow,
                    Role = currentUserRole
                });
            }
            // لو مش متعيّن لسيلز، يبقى مع اللي أنشأه (الماركتينج)
            if (!dto.AssignToSalesTeam)
                customer.AssignedToId = currentUserId;

            customer.CreatedById = currentUserId;
            await _unitOfWork.Repository<Customer>().AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<CustomerResponseDto>(customer);
            return result;

        }

        public async Task<IReadOnlyList<CustomerListItemDto>> SearchCustomersAsync(string? searchTerm)
        {
            // ?? "" معناها لو searchTerm = null خليه string فاضي
            var spec = new CustomersBySearchSpec(searchTerm ?? "");

            var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);

            return _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);
        }
        public List<StatusClientDto> GetCustomerStatuses()
        {
            return Enum.GetValues<StatusOfCustomers>()
                .Select(s => new StatusClientDto
                {
                    Id = (int)s,
                    Name = GetStatusArabicName(s)
                })
                .ToList();
        }

        private string GetStatusArabicName(StatusOfCustomers status)
        {
            return status switch
            {
                StatusOfCustomers.New => "جديد",
                StatusOfCustomers.None => "بدون حالة",
                StatusOfCustomers.Negotiating => "تفاوض",
                StatusOfCustomers.Buyer => "مشتري",
                StatusOfCustomers.NotBuyer => "غير مشتري",
                StatusOfCustomers.AssignedToSalesTeam => "محول لفريق المبيعات",
                StatusOfCustomers.Contacted => "تم التواصل",
                StatusOfCustomers.NoResponse => "لا يرد",
                StatusOfCustomers.SentQuote => "تم إرسال عرض سعر",
                StatusOfCustomers.TransferredToSupport => "محول للدعم",
                _ => "غير معروف"
            };
        }

        public async Task<Pagination<CustomerListItemDto>> GetAllCustomersAsync(
    CustomerPaginationParams paginationParams, string currentUserId, bool isAdmin)
        {
            // الأدمن يشوف الكل (null)، الماركتينج يشوف اللي هو أنشأه بس
            var ownerId = isAdmin ? null : currentUserId;

            //var spec = new CustomersWithPaginationSpec(paginationParams);


            //var countSpec = new Infrastructure.Specsification.CustomersCountSpec(paginationParams);
            ////var countSpec = new CustomersCountSpec();


            //var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);

            //var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);

            //var data = _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);

            //// 6) رجّع Pagination
            //return new Pagination<CustomerListItemDto>(
            //    paginationParams.PageIndex,
            //    paginationParams.PageSize,
            //    totalCount,
            //    data
            //);
            var spec = new CustomersWithPaginationSpec(paginationParams, ownerId);

            var countSpec = new CustomersCountSpec(paginationParams, ownerId);

            var totalCount = await _unitOfWork
                .Repository<Customer>()
                .CountAsync(countSpec);

            var query = _unitOfWork
                .Repository<Customer>()
                .ApplySpecification(spec);

            var data = await query
                .ProjectTo<CustomerListItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
            if (data.Count > 0)
            {
                var customerIds = data.Select(d => d.Id).ToList();

                var notesSpec = new CustomerNotesByUserForCustomersSpec(customerIds);
                var allNotes = await _unitOfWork.Repository<CustomerNote>().ListAsync(notesSpec);

                // كل عميل عنده ريكورد واحد فقط، فالـ dictionary مباشر
                var notesMap = allNotes
       .GroupBy(n => n.CustomerId)
       .ToDictionary(
           g => g.Key,
           g => g.OrderByDescending(x => x.CreatedAt)
                 .First()
       );

                foreach (var dto in data)
                {
                    if (notesMap.TryGetValue(dto.Id, out var note))
                    {
                        dto.NoteMarketing = note.NoteMarketing;
                        dto.NoteSales = note.NoteSales;
                        dto.NoteSupport = note.NoteSupport;
                    }
                }
            }
  

            return new Pagination<CustomerListItemDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                totalCount,
                data
            );

        }
        public async Task<CustomerListItemDto> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
        {
            // 1) جيب العميل (مع البيانات المرتبطة)
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 2) تحقق من الحملة لو متبعتة
            if (dto.CampaignId.HasValue && dto.CampaignId.Value > 0)
            {
                var campaign = await _unitOfWork.Repository<Campaigns>()
                    .GetEntityWithSpec(new CampaignByIdSpec(dto.CampaignId.Value));

                if (campaign == null)
                    throw new KeyNotFoundException(
                        $"Campaign with id {dto.CampaignId} not found");
            }

            // 3) عدّل البيانات الأساسية
            _mapper.Map(dto, customer);

            // 4) عدّل الخدمات (احذف القديم وضيف الجديد)
            if (dto.ServiceIds != null)
            {
                // تحقق إن كل الـ IDs موجودة
                var existingServices = await _unitOfWork.Repository<Service>()
                    .ListAsync(new ServicesByIdsSpec(dto.ServiceIds));

                var validIds = existingServices.Select(s => s.Id).ToList();
                var missing = dto.ServiceIds.Except(validIds).ToList();

                if (missing.Any())
                    throw new KeyNotFoundException(
                        $"Services not found: {string.Join(", ", missing)}");

                // احذف العلاقات القديمة فعليًا من الـ DbContext
                _unitOfWork.Repository<CustomerServices>()
                    .RemoveRange(customer.customerServices);

                // أعد إنشاء العلاقات الجديدة
                customer.customerServices = validIds
                    .Select(sid => new CustomerServices
                    {
                        customerId = customer.Id,
                        ServiceId = sid
                    })
                    .ToList();

            }

            // 5) احفظ
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            // 6) رجّع البيانات بعد التعديل
            return _mapper.Map<CustomerListItemDto>(customer);
        }
        public async Task DeleteCustomerAsync(int id)
        {
            // 1) جيب العميل
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

            // 2) لو مش موجود، رمي exception
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 3) احذف
            _unitOfWork.Repository<Customer>().Delete(customer);

            // 4) احفظ التغييرات
            await _unitOfWork.SaveChangesAsync();
        }
        // helper لجلب role المستخدم
        private async Task<string?> GetUserRoleAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user);
            return roles.FirstOrDefault();
        }
        public async Task<CustomerListItemDto> AssignToSalesPersonAsync(int id, string salesPersonId, string currentUserId, string currentUserRole)
        {
            // 1) Validation
            if (string.IsNullOrWhiteSpace(salesPersonId))
                throw new InvalidOperationException("SalesPersonId is required");

            // 2) جيب العميل (مع البيانات المرتبطة)
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 3) جيب المندوب من جدول AppUser
            var salesPerson = await _userManager.FindByIdAsync(salesPersonId);

            if (salesPerson == null)
                throw new KeyNotFoundException($"User with id {salesPersonId} not found");

            // 4) تحقق إنه فعلاً Sales Person (مش Admin أو HR)
            var isSales = await _userManager.IsInRoleAsync(salesPerson, "Sales");
            if (!isSales)
                throw new InvalidOperationException(
                    $"User '{salesPerson.UserName}' is not a sales person and cannot be assigned customers");
            // 5) لو العميل أصلاً معين لنفس الشخص
            if (customer.AssignedToId == salesPersonId)
                throw new InvalidOperationException("Customer is already assigned to this sales person");

            customer.AssignmentHistory.Add(new CustomerAssignmentHistory
            {
                FromUserId = customer.AssignedToId,  // المسؤول الحالي قبل التحويل
                FromRole = customer.AssignedToId != null ? await GetUserRoleAsync(customer.AssignedToId) : currentUserRole,
                ToUserId = salesPersonId,
                ToRole = UserRoles.Sales,
                AssignedAt = DateTime.UtcNow
            });


            customer.IsMarketingToSales=true;   




            // 5) عيّن العميل للمندوب
            customer.AssignedToId = salesPersonId;

            customer.status = StatusOfCustomers.AssignedToSalesTeam;  // ⚠️ غيري للحالة المناسبة عندك (مثلاً "معين لفريق المبيعات")

            // 6) احفظ
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            // 7) رجّع بيانات العميل المحدّثة
            return _mapper.Map<CustomerListItemDto>(customer);
        }
        // الدعم يرجّع العميل لمندوب المبيعات اللي حوّله للدعم أصلاً (من الـ AssignmentHistory)
        public async Task<CustomerListItemDto> ReturnCustomerToSalesAsync(
            int id, string currentUserId, string currentUserRole)
        {
            // 1) جيب العميل (مع الـ AssignmentHistory)
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 2) دوّر على مندوب المبيعات اللي حوّل العميل للدعم (أحدث تحويل Sales -> Support)
            var salesPersonId = customer.AssignmentHistory
                .Where(h => h.ToRole == UserRoles.Support
                            && h.FromRole == UserRoles.Sales
                            && h.FromUserId != null)
                .OrderByDescending(h => h.AssignedAt)
                .Select(h => h.FromUserId)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(salesPersonId))
                throw new InvalidOperationException(
                    "Cannot determine the sales person who transferred this customer to support.");

            // 3) تأكد إن المندوب لسه موجود ودوره Sales
            var salesPerson = await _userManager.FindByIdAsync(salesPersonId);
            if (salesPerson == null)
                throw new KeyNotFoundException($"User with id {salesPersonId} not found");

            var isSales = await _userManager.IsInRoleAsync(salesPerson, "Sales");
            if (!isSales)
                throw new InvalidOperationException(
                    $"User '{salesPerson.UserName}' is not a sales person anymore");

            // 4) لو العميل أصلاً معين لنفس الشخص
            if (customer.AssignedToId == salesPersonId)
                throw new InvalidOperationException("Customer is already assigned to this sales person");

            // 5) سجل في الـ History: من الدعم الحالي رجوعاً للمبيعات
            customer.AssignmentHistory.Add(new CustomerAssignmentHistory
            {
                FromUserId = customer.AssignedToId,
                FromRole = customer.AssignedToId != null
                    ? await GetUserRoleAsync(customer.AssignedToId)
                    : currentUserRole,
                ToUserId = salesPersonId,
                ToRole = UserRoles.Sales,
                AssignedAt = DateTime.UtcNow
            });

            // 6) سيب الفلاج زي ما هو
            customer.IsSupportToSales = true;
            customer.IsConsulted = true;

            // 7) رجّع العميل للمندوب وحدّث الـ AssignedToId والحالة
            customer.AssignedToId = salesPersonId;
            customer.status = StatusOfCustomers.AssignedToSalesTeam;

            // 8) احفظ
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerListItemDto>(customer);
        }
        public async Task<CustomerDetailsDto> GetCustomerByIdAsync(int id, string userId, string role, bool isAdmin)
        {
            // 1) جيب العميل مع البيانات المرتبطة
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            // 2) لو مش موجود
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 3) فحص الملكية: الأدمن يشوف الكل، غيره حسب الرول
            if (!isAdmin)
            {
                bool allowed = role switch
                {
                    UserRoles.Sales => customer.AssignedToId == userId,
                    UserRoles.Support => customer.AssignedToId == userId,
                    UserRoles.Marketing => customer.CreatedById == userId,
                    _ => false
                };

                if (!allowed)
                    throw new UnauthorizedAccessException("This customer does not belong to you");
            }

            // 4) Map للـ DTO
            return _mapper.Map<CustomerDetailsDto>(customer);
        }

        public async Task<Pagination<CustomerListItemDto>> GetAllgetSalesCustomers(CustomerPaginationParams paginationParams, string? currentUserId, bool isSalesOnly)
        {
            var spec = new SalesCustomersWithPaginationSpec(paginationParams, currentUserId, isSalesOnly);
            var countSpec = new SalesCustomersCountSpec(paginationParams, currentUserId, isSalesOnly);
            var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);
            if (data.Count > 0)
            {
                var customerIds = data.Select(d => d.Id).ToList();

                var notesSpec = new CustomerNotesByUserForCustomersSpec(customerIds);
                var allNotes = await _unitOfWork.Repository<CustomerNote>().ListAsync(notesSpec);

                // كل عميل عنده ريكورد واحد فقط، فالـ dictionary مباشر
                var notesMap = allNotes
       .GroupBy(n => n.CustomerId)
       .ToDictionary(
           g => g.Key,
           g => g.OrderByDescending(x => x.CreatedAt)
                 .First()
       );

                foreach (var dto in data)
                {
                    if (notesMap.TryGetValue(dto.Id, out var note))
                    {
                        dto.NoteMarketing = note.NoteMarketing;
                        dto.NoteSales = note.NoteSales;
                        dto.NoteSupport = note.NoteSupport;
                    }
                }
            }

                //         var notesMap = myNotes
                //.GroupBy(n => n.CustomerId)
                //.ToDictionary(
                //    g => g.Key,
                //     g => new
                //     {
                //         NoteMarketing = g
                //          .Where(x => x.MarketingCreatedById == currentUserId)
                //             .OrderByDescending(x => x.CreatedAt)
                //             .Select(x => x.NoteMarketing)
                //             .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),

                //         NoteSales = g
                //             .Where(x => x.SalesCreatedById == currentUserId)
                //             .OrderByDescending(x => x.CreatedAt)
                //             .Select(x => x.NoteSales)
                //             .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x)),

                //         NoteSupport = g
                //          .Where(x => x.SupportCreatedById == currentUserId)
                //             .OrderByDescending(x => x.CreatedAt)
                //             .Select(x => x.NoteSupport)
                //             .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x))
                //     });
                //         foreach (var dto in data)
                //         {
                //             if (notesMap.TryGetValue(dto.Id, out var notes))
                //             {
                //                 dto.NoteMarketing = notes.NoteMarketing;
                //                 dto.NoteSales = notes.NoteSales;
                //                 dto.NoteSupport = notes.NoteSupport;
                //             }
                //         }
                //     }
                return new Pagination<CustomerListItemDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                totalCount,
                data
            );

            
        }
        public async Task<Pagination<CustomerListItemDto>> GetAllgetSupportCustomers(CustomerPaginationParams paginationParams, string? currentUserId, bool isSupportOnly)
        {
            var spec = new SupportCustomersWithPaginationSpec(paginationParams, currentUserId, isSupportOnly);
            var countSpec = new SupportCustomersCountSpec(paginationParams, currentUserId, isSupportOnly);
            var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);
            if (!string.IsNullOrWhiteSpace(currentUserId) && data.Count > 0)
            {
                var customerIds = data.Select(d => d.Id).ToList();

                var notesSpec = new CustomerNotesByUserForCustomersSpec(customerIds);
                var allNotes = await _unitOfWork.Repository<CustomerNote>().ListAsync(notesSpec);

                // كل عميل عنده ريكورد واحد فقط، فالـ dictionary مباشر
                var notesMap = allNotes
        .GroupBy(n => n.CustomerId)
        .ToDictionary(
            g => g.Key,
            g => g.OrderByDescending(x => x.CreatedAt)
                  .First()
        );

                foreach (var dto in data)
                {
                    if (notesMap.TryGetValue(dto.Id, out var note))
                    {
                        dto.NoteMarketing = note.NoteMarketing;
                        dto.NoteSales = note.NoteSales;
                        dto.NoteSupport = note.NoteSupport;
                    }
                }
               

            }
            return new Pagination<CustomerListItemDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                totalCount,
                data
            );

        }
        public async Task UpdateCustomerNoteAsync(int customerId, string note, string userId, string role)
        {
            var customer = await _unitOfWork.Repository<Customer>()
                .GetByIdAsync(customerId);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {customerId} not found");

            if (string.IsNullOrWhiteSpace(role))
                throw new InvalidOperationException("User role could not be determined from token");
            // البحث عن ملاحظة موجودة لهذا العميل
            var customerNote = await _unitOfWork.Repository<CustomerNote>()
                .FirstOrDefaultAsync(n => n.CustomerId == customerId);
            var now = DateTime.UtcNow;
            // إذا لا توجد ملاحظة، أنشئ سجل جديد
            if (customerNote == null)
            {
                customerNote = new CustomerNote
                {
                    CustomerId = customerId,
                    Role = role
                };

                ApplyNoteByRole(customerNote, role, note, userId, now);
                await _unitOfWork.Repository<CustomerNote>()
                    .AddAsync(customerNote);
            }
            else
            {
                
                ApplyNoteByRole(customerNote, role, note, userId, now);
                _unitOfWork.Repository<CustomerNote>()
                    .Update(customerNote);
            }

            await _unitOfWork.SaveChangesAsync();

            
        }
        private static void ApplyNoteByRole(
    CustomerNote target, string role, string note, string userId, DateTime now)
        {
            switch (role.Trim().ToLower())
            {
                case "marketing":
                    target.NoteMarketing = note;
                    target.MarketingCreatedById = userId;
                    target.MarketingCreatedAt = now;
                    break;

                case "sales":
                    target.NoteSales = note;
                    target.SalesCreatedById = userId;
                    target.SalesCreatedAt = now;
                    break;

                case "support":
                    target.NoteSupport = note;
                    target.SupportCreatedById = userId;
                    target.SupportCreatedAt = now;
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported role: {role}");
            }
        }
        public async Task<IReadOnlyList<CustomerDropdownDto>> GetCustomersForDropdownAsync()
        {
            //var spec = new CustomersDropdownSpec();
            var customers = await _unitOfWork.Repository<Customer>().ListWithSelectAsync(spec: null, x => new CustomerDropdownDto
            {
                Id = x.Id,
                FullName = x.FullName,
                CampanyName = x.CampanyName,
                Services = x.customerServices.Select(cs => new ServiceDropdownDto
                {
                    Id = cs.Service.Id,
                    Name = cs.Service.NameAr
                }).ToList()
            });
            return customers;
            //return _mapper.Map<IReadOnlyList<CampaignDropdownDto>>(campaigns);
        }

        public async Task<CustomerListItemDto> AssignToSupportPersonAsync(int id, string SupportPersonId, string currentUserId, string currentUserRole)
        {
            // 1) Validation
            if (string.IsNullOrWhiteSpace(SupportPersonId))
                throw new InvalidOperationException("SupportPersonId is required");

            // 2) جيب العميل (مع البيانات المرتبطة)
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 3) جيب المندوب من جدول AppUser
            var SupportPerson = await _userManager.FindByIdAsync(SupportPersonId);

            if (SupportPerson == null)
                throw new KeyNotFoundException($"User with id {SupportPersonId} not found");

            // 4) تحقق إنه فعلاً Sales Person (مش Admin أو HR)
            var isSupport = await _userManager.IsInRoleAsync(SupportPerson, "Support");
            if (!isSupport)
                throw new InvalidOperationException(
                    $"User '{SupportPerson.UserName}' is not a Support person and cannot be assigned customers");
            // 5) لو العميل أصلاً معين لنفس الشخص
            if (customer.AssignedToId == SupportPersonId)
                throw new InvalidOperationException("Customer is already assigned to this support person");

            // 6) ✅ سجل في الـ AssignmentHistory
            customer.AssignmentHistory.Add(new CustomerAssignmentHistory
            {
                FromUserId = customer.AssignedToId,
                FromRole = customer.AssignedToId != null
                    ? await GetUserRoleAsync(customer.AssignedToId)
                    : currentUserRole,
                ToUserId = SupportPersonId,
                ToRole = UserRoles.Support,
                AssignedAt = DateTime.UtcNow
            });
            // 6) ✅ حدّدي الـ FromRole
            //var fromRole = customer.AssignedToId != null
            //    ? await GetUserRoleAsync(customer.AssignedToId)
            //    : currentUserRole;

            //// 7) ✅ أنشئي الـ history في متغير
            //var history = new CustomerAssignmentHistory
            //{
            //    FromUserId = customer.AssignedToId,
            //    FromRole = fromRole,
            //    ToUserId = SupportPersonId,
            //    ToRole = UserRoles.Support,
            //    AssignedAt = DateTime.UtcNow
            //};

            customer.IsSalesToSupport=true;   // ✅ بيحدد IsSalesToSupport تلقائياً

            //customer.AssignmentHistory.Add(history);

            // 7) عيّن العميل لموظف الدعم
            customer.AssignedToId = SupportPersonId;
            customer.status = StatusOfCustomers.TransferredToSupport;

            // 6) احفظ
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            // 7) رجّع بيانات العميل المحدّثة
            return _mapper.Map<CustomerListItemDto>(customer);
        }

        public async Task<CustomerResponseDto> UpdateCustomerStatusAsync(int id, UpdateCustomerStatusDto dto)
        {
            var spec = new CustomerWithNotesSpecification(id);
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(spec);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found.");

            // 2) تحقق إن الـ Status قيمة صحيحة في الـ Enum
            if (!Enum.IsDefined(typeof(StatusOfCustomers), dto.Status))
                throw new InvalidOperationException("Invalid customer status value.");
            // 3) لو الحالة الجديدة هي NotBuyer لازم سبب عدم الشراء يكون موجود
            if (dto.Status == StatusOfCustomers.NotBuyer)
            {
                if (string.IsNullOrWhiteSpace(dto.NotBuyingReason))
                    throw new InvalidOperationException(
                        "Reason for not buying is required.");
            }
            // 3) لو نفس الحالة الحالية، مفيش داعي للتحديث
            if (customer.status == dto.Status)
                throw new InvalidOperationException("Customer already has this status.");

            // 4) حدّث الحالة
            customer.status = dto.Status;

            // ✅ لو الحالة بقت Contacted، سجّل تاريخ المكالمة تلقائي
            // كده "مكالمات اليوم" في الداشبورد تتعد صح حتى لو العميل لسه جديد
            if (dto.Status == StatusOfCustomers.Contacted)
            {
                customer.LastFollowUpDate = DateTime.UtcNow;
            }

            if (dto.Status == StatusOfCustomers.NotBuyer)
            {
                customer.NotBuyingReason= dto.NotBuyingReason!.Trim();
            }
            else
            {
                // لو غير الحالة إلى أي حالة أخرى، امسح السبب
                customer.NotBuyingReason = null;
            }

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            //// 5) رجّع العميل بعد التحديث (بالـ Spec علشان تجيب الـ Includes زي AssignedTo و Campaign)
            //var spec = new CustomerWithDetailsSpecification(id);
            //var updatedCustomer = await _unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);

            return _mapper.Map<CustomerResponseDto>(customer);
        }
        public async Task<CustomerResponseDto> UpdateCustomerFollowUpAsync(int id, UpdateCustomerFollowUpDto dto)
        {
            // 1) جيب العميل
            var spec = new CustomerWithNotesSpecification(id);
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(spec);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found.");

            // 2) تحقق إن المتابعة القادمة بعد آخر متابعة
            if (dto.LastFollowUpDate.HasValue && dto.NextFollowUpDate.HasValue
                && dto.NextFollowUpDate.Value <= dto.LastFollowUpDate.Value)
            {
                throw new InvalidOperationException("Next follow-up date must be after the last follow-up date.");
            }

            // 3) حدّث الحقول
            customer.LastFollowUpDate = dto.LastFollowUpDate;
            customer.NextFollowUpDate = dto.NextFollowUpDate;

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();


            return _mapper.Map<CustomerResponseDto>(customer);
        }
        // ✅ Upsert: لو المستخدم عنده ملاحظة على العميل ده، عدّلها. لو لأ، ضيف جديدة
        public async Task<CustomerNoteResponseDto> UpsertMyNoteAsync(
            int customerId, string note, string userId, string role)
        {
            if (string.IsNullOrWhiteSpace(note))
                throw new InvalidOperationException("Note content cannot be empty.");
            if (string.IsNullOrWhiteSpace(role))
                throw new InvalidOperationException("User role could not be determined.");
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("User id could not be determined.");

            // تأكد إن العميل موجود
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {customerId} not found.");
            // (2) يُسمح بالتعديل لو: Admin، أو الشخص المعيَّن حالياً، أو اللي أنشأ العميل
            var isAdmin = string.Equals(role.Trim(), UserRoles.Admin, StringComparison.OrdinalIgnoreCase);
            var isAssignee = string.Equals(customer.AssignedToId, userId, StringComparison.Ordinal);
            //var isCreator = string.Equals(customer.CreatedById, userId, StringComparison.Ordinal);

            //if (!isAdmin && !isAssignee && !isCreator)
            if (!isAdmin && !isAssignee)
            {
                throw new InvalidOperationException(
                    "You are not allowed to add or update a note for this customer. " +
                    "Only the current assignee, the customer's creator, or an admin can do that.");
            }

                // ابحث عن ملاحظة موجودة لنفس المستخدم على نفس العميل
                var spec = new CustomerNoteByUserAndCustomerSpec(customerId, userId);
            var existingNote = await _unitOfWork.Repository<CustomerNote>().GetEntityWithSpec(spec);

            var now = DateTime.UtcNow;
            CustomerNote resultNote;

            if (existingNote == null)
            {
                
                resultNote = new CustomerNote
                {
                    CustomerId = customerId,
                    Role = role
                };

                ApplyNoteByRole(resultNote, role, note, userId, now);
                await _unitOfWork.Repository<CustomerNote>()
                    .AddAsync(resultNote);
            }
            else
            {
                // الريكورد موجود → حدّثي بس section الـ role الحالي
                ApplyNoteByRole(existingNote, role, note, userId, now);


                

                _unitOfWork.Repository<CustomerNote>()
         .Update(existingNote);

                resultNote = existingNote;

            }

            await _unitOfWork.SaveChangesAsync();
            // إعادة تحميل الكيان بكل الـ navigation properties
            var responseSpec = new CustomerNoteByIdSpec(resultNote.Id);
            var savedNote = await _unitOfWork.Repository<CustomerNote>()
                .GetEntityWithSpec(responseSpec);

            return _mapper.Map<CustomerNoteResponseDto>(savedNote);
      
        }
        
        public async Task<IReadOnlyList<CustomerNoteResponseDto>> GetNotesByCustomerAsync(
       int customerId, string? role = null)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {customerId} not found.");

            var spec = new CustomerNotesByCustomerSpecification(customerId, role);
            var notes = await _unitOfWork.Repository<CustomerNote>().ListAsync(spec);

            return _mapper.Map<IReadOnlyList<CustomerNoteResponseDto>>(notes);
        }

        public async Task<SalesDashboardStatsDto> GetSalesDashboardStatsAsync(string? salesPersonId)
        {
            // الأدمن: salesPersonId = null → بيرجع إحصائيات الكل
            // السيلز: salesPersonId = userId → بيرجع إحصائياته هو بس

            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

       
       
            
            var BuyerCustomers = await _unitOfWork.Repository<Customer>()
             .CountAsync(new BuyerCustomersBySalesPersonSpec(salesPersonId));

            // 2) مكالمات النهاردة (متابعة آخر تاريخها = النهاردة)
            var callsToday = await _unitOfWork.Repository<Customer>()
           .CountAsync(new CallsTodayBySalesPersonSpec(salesPersonId, today, tomorrow));

            // 3) متابعات قادمة (موعدها من النهاردة وللأمام)
            var upcomingFollowUps = await _unitOfWork.Repository<Customer>()
                   .CountAsync(new UpcomingFollowUpsSpec(salesPersonId, today));

            // 4) متابعات فاتت موعدها
            var overdueFollowUps = await _unitOfWork.Repository<Customer>()
     .CountAsync(new OverdueFollowUpsSpec(salesPersonId, today));

            // 5) و 6) التحويلات من AssignmentHistory
            // 5) عملاء حولّتهم لـ Support
            var transferredToSupport = await _unitOfWork.Repository<CustomerAssignmentHistory>()
                .CountAsync(new TransferredToSupportBySalesPersonSpec(salesPersonId));

            // 6) عملاء استلمتهم من Marketing
            var receivedFromMarketing = await _unitOfWork.Repository<CustomerAssignmentHistory>()
                .CountAsync(new ReceivedFromMarketingBySalesPersonSpec(salesPersonId));

            return new SalesDashboardStatsDto
            {
                BuyerCustomers = BuyerCustomers,
                CallsToday = callsToday,
                UpcomingFollowUps = upcomingFollowUps,
                OverdueFollowUps = overdueFollowUps,
                TransferredToSupport = transferredToSupport,
                ReceivedFromMarketing = receivedFromMarketing
            };

        }

        public async Task<List<string>> GetNotBuyingReasonsAsync()
        {
            var spec = new NotBuyingReasonsSpecification();

            // استخدام ListWithSelectAsync الموجود بالفعل في GenericRepository
            var reasons = await _unitOfWork.Repository<Customer>()
                .ListWithSelectAsync(
                    spec,
                    c => c.NotBuyingReason!
                );

            return reasons
      .Distinct(StringComparer.OrdinalIgnoreCase)
      .ToList();
        }
        public async Task<List<CustomerStatusCountDto>> GetCustomerCountByStatusAsync(string currentUserId, bool isAdmin)
        {
            var spec = new CustomerCountByStatusSpec(currentUserId, isAdmin);

            var customers = await _unitOfWork
                .Repository<Customer>()
                .ListAsync(spec);

            var result = customers
                .GroupBy(c => c.status)
                .Select(g => new CustomerStatusCountDto
                {
                    StatusId = (int)g.Key,
                    StatusName = g.Key.ToString(),
                    Count = g.Count()
                })
                .OrderBy(x => x.StatusId)
                .ToList();

            return result;
        }

    }


}
