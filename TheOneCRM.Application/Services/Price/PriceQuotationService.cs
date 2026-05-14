using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Application.Interfaces.IPriceQuotation;
using TheOneCRM.Domain.Interfaces;
using TheOneCRM.Domain.Models.DTOs.PriceQuotationsDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Infrastructure.Specsification;
using TheOneCRM.Infrastructure.Specsification.Price;

namespace TheOneCRM.Application.Services.price
{
    public class PriceQuotationService : IPriceQuotationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PriceQuotationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PriceQuotationResponseDto> CreatePriceQuotationAsync(CreatePriceQuotationDto dto)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId);
            if (customer is null)
                throw new KeyNotFoundException($"Customer Id{dto.CustomerId}Not Found");

            var serviceIds = dto.Items.Select(i => i.ServiceId).Distinct().ToList();
            var servicesSpec = new ServicesByIdsSpec(serviceIds);
            var services = await _unitOfWork.Repository<Service>().ListAsync(servicesSpec);
            if (services.Count != serviceIds.Count)
            {
                var missingIds = serviceIds.Except(services.Select(s => s.Id)).ToList();
                throw new KeyNotFoundException($"الخدمات التالية غير موجودة: {string.Join(", ", missingIds)}");
            }

            // 3) بناء البنود وحساب TotalPrice لكل بند
            var items = dto.Items.Select(i => new PriceQuotationDetails
            {
                ServiceId = i.ServiceId,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.UnitPrice * i.Quantity   // الإجمالي لكل بند
            }).ToList();
            // 4) حساب الـ SubTotal
            var subTotal = items.Sum(i => i.TotalPrice);
            // 5) التحقق إن الخصم مش أكبر من المجموع
            if (dto.Discount > subTotal)
                throw new InvalidOperationException("قيمة الخصم لا يمكن أن تتجاوز المجموع قبل الخصم");

            // 6) حساب الصافي
            var netTotal = subTotal - dto.Discount;

            // 7) إنشاء عرض السعر
            var priceQuotation = new PriceQuotation
            {
                CustomerId = dto.CustomerId,
                SubTotal = subTotal,
                Discount = dto.Discount,
                NetTotal = netTotal,
                Notes = dto.Notes,
                Items = items
            };
            await _unitOfWork.Repository<PriceQuotation>().AddAsync(priceQuotation);
            await _unitOfWork.SaveChangesAsync();
            // 8) تجهيز الـ Response
            return new PriceQuotationResponseDto
            {
                Id = priceQuotation.Id,
                CustomerId = customer.Id,
                CustomerName = customer.FullName,
                SubTotal = priceQuotation.SubTotal,
                Discount = priceQuotation.Discount,
                NetTotal = priceQuotation.NetTotal,
                Notes = priceQuotation.Notes,
                Items = priceQuotation.Items.Select(i => new PriceQuotationDetailsResponseDto
                {
                    Id = i.Id,
                    ServiceId = i.ServiceId,
                    ServiceName = services.First(s => s.Id == i.ServiceId).NameAr,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
            public async Task<PaginatedPriceQuotationsDto> GetAllPriceQuotationsAsync(PriceQuotationParams p)
        {
            // 1) جلب البيانات مع الـ paging
            var spec = new PriceQuotationsFilterSpec(p);
            var quotations = await _unitOfWork.Repository<PriceQuotation>().ListAsync(spec);

            // 2) جلب العدد الكلي (بدون paging)
            var countSpec = new PriceQuotationsCountSpec(p);
            var totalCount = await _unitOfWork.Repository<PriceQuotation>().CountAsync(countSpec);

            // 3) تحويل للـ DTO
            var data = quotations.Select(q => new PriceQuotationListDto
            {
                Id = q.Id,
                CustomerId = q.CustomerId,
                CustomerName = q.Customer?.FullName ?? string.Empty,
                CampanyName=q.Customer.CampanyName??String.Empty,
                SubTotal = q.SubTotal,
                Discount = q.Discount,
                NetTotal = q.NetTotal,
                ItemsCount = q.Items?.Count ?? 0,
                Notes = q.Notes
            }).ToList();

            // 4) Response
            return new PaginatedPriceQuotationsDto
            {
                TotalCount = totalCount,
                PageIndex = p.PageIndex,
                PageSize = p.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)p.PageSize),
                Data = data
            };
        }
        public async Task<PriceQuotationResponseDto> GetPriceQuotationByIdAsync(int id)
        {
            // 1) جلب عرض السعر مع التفاصيل
            var spec = new PriceQuotationByIdSpec(id);
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetEntityWithSpec(spec);

            // 2) التأكد من وجوده
            if (quotation is null)
                throw new KeyNotFoundException($"عرض السعر برقم {id} غير موجود");

            // 3) تحويل للـ Response DTO
            return new PriceQuotationResponseDto
            {
                Id = quotation.Id,
                CustomerId = quotation.CustomerId,
                CustomerName = quotation.Customer?.FullName ?? string.Empty,
                SubTotal = quotation.SubTotal,
                Discount = quotation.Discount,
                NetTotal = quotation.NetTotal,
                CampanyName=quotation.Customer.CampanyName,
                Notes = quotation.Notes,
                Items = quotation.Items.Select(i => new PriceQuotationDetailsResponseDto
                {
                    Id = i.Id,
                    ServiceId = i.ServiceId,
                    ServiceName = i.Service?.NameAr ?? string.Empty,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
        public async Task<PriceQuotationResponseDto> UpdatePriceQuotationAsync(int id, UpdatePriceQuotationDto dto)
        {
            // 1) جلب عرض السعر مع التفاصيل
            var spec = new PriceQuotationByIdSpec(id);
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetEntityWithSpec(spec);

            if (quotation is null)
                throw new KeyNotFoundException($"عرض السعر برقم {id} غير موجود");

            // 2) التأكد من العميل
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(dto.CustomerId);
            if (customer is null)
                throw new KeyNotFoundException($"العميل برقم {dto.CustomerId} غير موجود");

            // 3) التأكد من الخدمات
            var serviceIds = dto.Items.Select(i => i.ServiceId).Distinct().ToList();
            var servicesSpec = new ServicesByIdsSpec(serviceIds);
            var services = await _unitOfWork.Repository<Service>().ListAsync(servicesSpec);

            if (services.Count != serviceIds.Count)
            {
                var missingIds = serviceIds.Except(services.Select(s => s.Id)).ToList();
                throw new KeyNotFoundException($"الخدمات التالية غير موجودة: {string.Join(", ", missingIds)}");
            }

            // 4) حذف البنود القديمة كلها
            foreach (var oldItem in quotation.Items.ToList())
            {
                _unitOfWork.Repository<PriceQuotationDetails>().Delete(oldItem);
            }

            // 5) إنشاء البنود الجديدة
            var newItems = dto.Items.Select(i => new PriceQuotationDetails
            {
                PriceQuotationId = quotation.Id,
                ServiceId = i.ServiceId,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
                TotalPrice = i.UnitPrice * i.Quantity
            }).ToList();

            // 6) حساب الإجماليات
            var subTotal = newItems.Sum(i => i.TotalPrice);

            if (dto.Discount > subTotal)
                throw new InvalidOperationException("قيمة الخصم لا يمكن أن تتجاوز المجموع قبل الخصم");

            // 7) تحديث بيانات عرض السعر
            quotation.CustomerId = dto.CustomerId;
            quotation.SubTotal = subTotal;
            quotation.Discount = dto.Discount;
            quotation.NetTotal = subTotal - dto.Discount;
            quotation.Notes = dto.Notes;
            quotation.Items = newItems;

            _unitOfWork.Repository<PriceQuotation>().Update(quotation);
            await _unitOfWork.SaveChangesAsync();

            // 8) Response
            return new PriceQuotationResponseDto
            {
                Id = quotation.Id,
                CustomerId = customer.Id,
                CustomerName = customer.FullName,
                SubTotal = quotation.SubTotal,
                Discount = quotation.Discount,
                NetTotal = quotation.NetTotal,
                Notes = quotation.Notes,
                Items = quotation.Items.Select(i => new PriceQuotationDetailsResponseDto
                {
                    Id = i.Id,
                    ServiceId = i.ServiceId,
                    ServiceName = services.First(s => s.Id == i.ServiceId).NameAr,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
        public async Task DeletePriceQuotationAsync(int id)
        {
            // 1) جلب عرض السعر مع البنود
            var spec = new PriceQuotationByIdSpec(id);
            var quotation = await _unitOfWork.Repository<PriceQuotation>().GetEntityWithSpec(spec);

            // 2) التأكد من وجوده
            if (quotation is null)
                throw new KeyNotFoundException($"عرض السعر برقم {id} غير موجود");

            // 3) حذف البنود الأول (لو مفيش Cascade Delete مفعّل)
            foreach (var item in quotation.Items.ToList())
            {
                _unitOfWork.Repository<PriceQuotationDetails>().Delete(item);
            }

            // 4) حذف عرض السعر نفسه
            _unitOfWork.Repository<PriceQuotation>().Delete(quotation);

            // 5) حفظ التغييرات
            await _unitOfWork.SaveChangesAsync();
        }
    }
    }

