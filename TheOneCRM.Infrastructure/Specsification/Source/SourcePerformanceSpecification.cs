using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOneCRM.Domain.Models.Entities;
using TheOneCRM.Domain.Specifications;

namespace TheOneCRM.Infrastructure.Specsification.Source
{
    public class SourcePerformanceSpecification :BaseSpecification<Campaigns>
    {
        public SourcePerformanceSpecification()
        {
            //Includes.Add(c => c.ChannelSource);
           
        }
    }
}
