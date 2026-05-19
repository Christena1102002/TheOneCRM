using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Contracts
{
    public class ContractStatisticsDto
    {
        public int TotalContracts { get; set; }       // إجمالي العقود
        public int ActiveContracts { get; set; }      // العقود النشطة
        public int ExpiredContracts { get; set; }     // العقود المنتهية
        public int ExpiringSoonContracts { get; set; } // تنتهي قريباً (خلال 30 يوم)
    }
}
