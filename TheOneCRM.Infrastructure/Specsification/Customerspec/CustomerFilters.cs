using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TheOneCRM.Domain.Models.DTOs.CustomerDtos;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public static class CustomerFilters
    {
        public static Expression<Func<Customer, bool>> Build(CustomerPaginationParams p)
        {
            return c =>

                //  Search (بالاسم أو الفون)
                (string.IsNullOrWhiteSpace(p.Search) ||

                    EF.Functions.Like(c.FullName, $"%{p.Search}%") ||

                    (c.Phone != null &&
                     EF.Functions.Like(c.Phone, $"%{p.Search}%"))
                )

                &&

                
                (!p.SourceId.HasValue ||

                    (c.campaigns != null &&
                     c.campaigns.ChannelSourceId == p.SourceId.Value)
                )

                &&

            
                (!p.CustomerStatusId.HasValue ||

                    (Enum.IsDefined(typeof(StatusOfCustomers), p.CustomerStatusId.Value) &&
                     c.status == (StatusOfCustomers)p.CustomerStatusId.Value)
                );
        }
    }
}