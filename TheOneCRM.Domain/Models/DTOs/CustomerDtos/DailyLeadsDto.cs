using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.CustomerDtos
{
    public class DailyLeadsDto
    {
        public string Date { get; set; }
        public int Count { get; set; }
    }
}
