using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class CustomerNoteResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? NoteMarketing { get; set; }
        public string MarketingRole { get; set; }
        public string? NoteSales { get; set; }
        public string SalesRole { get; set; }
        public string? NoteSupport { get; set; }
        public string SupportRole { get; set; }
        public string CreatedById { get; set; }
        public string? NoteMarketingName { get; set; }
        public string? NoteSalesName { get; set; }
       
        public string? NoteSupportName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
