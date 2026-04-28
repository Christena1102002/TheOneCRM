using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Projects :BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public StatusOfProject status { get; set; }
        public int Price{get; set; }
        public DateTime Start {  get; set; }
        public DateTime End {  get; set; }
        public int customerId { get; set; }
        [ForeignKey("customerId")]
        public Customers? customers { get; set; }

        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser AssignedTo { get; set; }

    }
}
