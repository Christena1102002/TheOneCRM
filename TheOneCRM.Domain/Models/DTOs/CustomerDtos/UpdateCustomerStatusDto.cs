using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class UpdateCustomerStatusDto
    {
        public StatusOfCustomers Status { get; set; }
    }
}
