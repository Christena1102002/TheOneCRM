using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneCRM.Domain.Models.DTOs.Articles
{
    // عنصر في قائمة "الفئة" بتاعة فورم المقالة — بيتملأ حسب رول المستخدم
    // Developer => مشاريعه، Support/Sales => عملاؤهم، Admin => الكل
    public class ArticleCategoryOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!; // "Project" أو "Customer"
    }
}
