using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CustomerByIdSpec : BaseSpecification<Customer>
    {
        public CustomerByIdSpec(int id) : base(c => c.Id == id)
        {
            AddInclude(c => c.campaigns.ChannelSource);
            AddInclude(c => c.AssignedTo);
            AddInclude("customerServices.Service");
        }
    }
}
