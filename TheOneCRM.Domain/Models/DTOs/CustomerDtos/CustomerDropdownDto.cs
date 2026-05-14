using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerDropdownDto
    {
        public int Id { get; set; }
        public string FullName { set; get; }
        public string CampanyName { set; get; }
        public List<ServiceDropdownDto> Services { get; set; } = new();
    }
    public class ServiceDropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
