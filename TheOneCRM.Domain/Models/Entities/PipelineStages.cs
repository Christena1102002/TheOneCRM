using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.Entities
{
    public class PipelineStages : BaseEntity
    {
        public string Name { get; set; } 
        public int Order { get; set; }

        public int Probability { get; set; }

        public StatusOfCustomers status { get; set; } //active
        // Relation مع Deals
        public List<Deal> Deals { get; set; } 
    }
}
