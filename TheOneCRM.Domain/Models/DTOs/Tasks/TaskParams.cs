using TheOneCRM.Domain.Models.Enums;

namespace TheOneCRM.Domain.Models.DTOs.Tasks
{
    public class TaskParams
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 10;

        public int PageIndex { get; set; } = 1;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? Search { get; set; }
        public int? ProjectId { get; set; }
        public StatusOfTask? Status { get; set; }
        public PriorityStatus? Priority { get; set; }
    }
}
