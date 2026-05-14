using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.CampaignsSpec;
using TheOneCRM.Infrastructure.Specsification.Customerspec;
using static TheOneCRM.Infrastructure.Specsification.CustomersWithFilterSpec;

namespace TheOneCRM.Application.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper= mapper;
            _userManager= userManager;
        }

        public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto, string currentUserId, string currentUserRole)
        {
           
            if (dto.CampaignId > 0)
            {
                var campaign = await _unitOfWork.Repository<Campaigns>()
                    .GetEntityWithSpec(new CampaignByIdSpec((int)dto.CampaignId));
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

                var validServiceIds = existingServices.Select(s => s.Id).ToList();
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
                customer.AssignedToId = dto.SalesPersonId;
                customer.status = StatusOfCustomers.AssignedToSalesTeam;


            }
            // 4) ✅ ضيف الملاحظة الأولية لو موجودة
            if (!string.IsNullOrWhiteSpace(dto.Notes))
            {
                customer.Notes.Add(new CustomerNote
                {
                    Note = dto.Notes,
                    CreatedById = currentUserId,
                    Role = currentUserRole
                });
            }
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
    CustomerPaginationParams paginationParams)
        {

            var spec = new CustomersWithPaginationSpec(paginationParams);


            var countSpec = new Infrastructure.Specsification.CustomersCountSpec(paginationParams);
            //var countSpec = new CustomersCountSpec();


            var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);

            var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);

            var data = _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);

            // 6) رجّع Pagination
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
            // 5) عيّن العميل للمندوب
            customer.AssignedToId = salesPersonId;

            customer.status = StatusOfCustomers.AssignedToSalesTeam;  // ⚠️ غيري للحالة المناسبة عندك (مثلاً "معين لفريق المبيعات")

            // 6) احفظ
            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();

            // 7) رجّع بيانات العميل المحدّثة
            return _mapper.Map<CustomerListItemDto>(customer);
        }
        public async Task<CustomerDetailsDto> GetCustomerByIdAsync(int id)
        {
            // 1) جيب العميل مع البيانات المرتبطة
            var customer = await _unitOfWork.Repository<Customer>()
                .GetEntityWithSpec(new CustomerByIdSpec(id));

            // 2) لو مش موجود
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            // 3) Map للـ DTO
            return _mapper.Map<CustomerDetailsDto>(customer);
        }

        public async Task<Pagination<CustomerListItemDto>> GetAllgetSalesCustomers(CustomerPaginationParams paginationParams,string? currentUserId,bool isSalesOnly)
        {
            var spec = new SalesCustomersWithPaginationSpec(paginationParams, currentUserId, isSalesOnly);
            var countSpec = new SalesCustomersCountSpec(paginationParams, currentUserId, isSalesOnly);
            var customers = await _unitOfWork.Repository<Customer>().ListAsync(spec);
            var totalCount = await _unitOfWork.Repository<Customer>().CountAsync(countSpec);
            var data = _mapper.Map<IReadOnlyList<CustomerListItemDto>>(customers);

            return new Pagination<CustomerListItemDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                totalCount,
                data
            );

        }

        public async Task UpdateCustomerNoteAsync(int customerId, string note, string userId)
        {
            var customer = await _unitOfWork.Repository<Customer>()
                .GetByIdAsync(customerId);

            if (customer == null)
                throw new Exception("Customer not found");

            var customerNote = new CustomerNote
            {
                CustomerId = customerId,
                Note = note,
                CreatedById = userId
            };

            _unitOfWork.Repository<Customer>().Update(customer);
            await _unitOfWork.SaveChangesAsync();
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
            // 1) جيب العميل
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {id} not found.");

            // 2) تحقق إن الـ Status قيمة صحيحة في الـ Enum
            if (!Enum.IsDefined(typeof(StatusOfCustomers), dto.Status))
                throw new InvalidOperationException("Invalid customer status value.");

            // 3) لو نفس الحالة الحالية، مفيش داعي للتحديث
            if (customer.status == dto.Status)
                throw new InvalidOperationException("Customer already has this status.");

            // 4) حدّث الحالة
            customer.status = dto.Status;

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
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(id);

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

            // تأكد إن العميل موجود
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer == null)
                throw new KeyNotFoundException($"Customer with id {customerId} not found.");

            // ابحث عن ملاحظة موجودة لنفس المستخدم على نفس العميل
            var spec = new CustomerNoteByUserAndCustomerSpec(customerId, userId);
            var existingNote = await _unitOfWork.Repository<CustomerNote>().GetEntityWithSpec(spec);

            CustomerNote resultNote;

            if (existingNote != null)
            {
                // تعديل الملاحظة الموجودة
                existingNote.Note = note;
                _unitOfWork.Repository<CustomerNote>().Update(existingNote);
                resultNote = existingNote;
            }
            else
            {
                // إضافة ملاحظة جديدة
                resultNote = new CustomerNote
                {
                    CustomerId = customerId,
                    Note = note,
                    CreatedById = userId,
                    Role = role
                };
                await _unitOfWork.Repository<CustomerNote>().AddAsync(resultNote);
            }

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CustomerNoteResponseDto>(resultNote);
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
       
    }
    

}
