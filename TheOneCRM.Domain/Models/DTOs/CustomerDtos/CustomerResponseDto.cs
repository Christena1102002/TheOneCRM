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
        public bool IsMarketingToSales { get; set; }
        public bool IsSalesToSupport { get; set; }
        public bool IsSupportToSales { get; set; }

        public string? NoteMarketing { get; set; }
        public string? NoteSales { get; set; }
        public string? NoteSupport { get; set; }
    }
}
