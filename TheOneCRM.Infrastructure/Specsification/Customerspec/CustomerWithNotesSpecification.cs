using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Customerspec
{
    public class CustomerWithNotesSpecification : BaseSpecification<Customer>
    {
        public CustomerWithNotesSpecification(int id)
            : base(c => c.Id == id)
        {
            Includes.Add(c => c.Notes);
        }
    }
}
