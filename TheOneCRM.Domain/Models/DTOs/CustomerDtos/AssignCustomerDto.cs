using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class AssignCustomerDto
    {
        public string SalesPersonId { get; set; }
    }
    public class AssignToSupportPersonDto
    {
        public string SupportPersonId { get; set; }
    }
}
