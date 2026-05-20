using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Models.Enums;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class BuyerCustomersBySalesPersonSpec:BaseSpecification<Customer>
    {
        public BuyerCustomersBySalesPersonSpec(string salesPersonId):base (c=>c.AssignedToId == salesPersonId && c.status==StatusOfCustomers.Buyer)
        {

        }
    }
}
