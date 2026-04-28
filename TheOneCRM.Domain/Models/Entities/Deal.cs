using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Deal  :BaseEntity
    {
        public string Title { get; set; } 
        public decimal Value { get; set; } 
       
        public StatusOfDeal statusOfDeal { get; set; }
        public int? customerId { get; set; }

        [ForeignKey("customerId")]
        public Customers? customer { get; set; }
        //public int StageId { get; set; }
        //public PipelineStages Stage { get; set; }

        //created by && assidnto
        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser AssignedTo { get; set; }
    }
}
