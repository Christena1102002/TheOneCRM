using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class Tasks : BaseEntity
    {
        public string Title { get; set; }
       
        public StatusOfTask Status { get; set; }
        public int? OrderIndex { get; set; }
        
        public string? Description { set; get; }
        public DateTime DueDate { get; set; }

        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects? Project { get; set; }

        public string? CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public AppUser CreatedBy { get; set; }
        public string? AssignedToId { get; set; }

        [ForeignKey("AssignedToId")]
        public AppUser AssignedTo { get; set; }
    }
}
