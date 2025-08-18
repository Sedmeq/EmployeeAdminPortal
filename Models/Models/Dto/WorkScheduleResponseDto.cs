using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dto
{
    public class WorkScheduleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int RequiredWorkHours { get; set; }
        public int MinimumWorkMinutes { get; set; }
        public int MaxLatenessMinutes { get; set; }
        public int MaxEarlyLeaveMinutes { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public int EmployeeCount { get; set; }
    }
}
