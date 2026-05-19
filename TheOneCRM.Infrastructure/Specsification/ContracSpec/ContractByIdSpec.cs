using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.ContracSpec
{
    public class ContractByIdSpec : BaseSpecification<Contract>
    {
        public ContractByIdSpec(int id) : base(x => x.Id == id)
        {
            // مفيش includes — ProjectTo هيتولّى الموضوع
        }
    }
}
