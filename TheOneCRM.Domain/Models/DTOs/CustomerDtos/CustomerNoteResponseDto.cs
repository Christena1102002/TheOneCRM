using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerNoteResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public string Role { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
