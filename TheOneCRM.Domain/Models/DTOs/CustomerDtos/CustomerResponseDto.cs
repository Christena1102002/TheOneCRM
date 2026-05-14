using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? LastFollowUpDate { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
        public List<CustomerNoteResponseDto> CustomerNotes { get; set; } = new();
    }
}
