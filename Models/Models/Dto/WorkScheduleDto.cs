using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dto
{
    public class WorkScheduleDto
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }

        [Required]
        public string StartTime { get; set; } = string.Empty; // "08:00" formatında

        [Required]
        public string EndTime { get; set; } = string.Empty; // "17:00" formatında

        [Range(1, 24)]
        public int RequiredWorkHours { get; set; } = 8;

        [Range(0, 1440)] // 0-24 saat arası dəqiqələrlə
        public int MinimumWorkMinutes { get; set; } = 480; // 8 saat = 480 dəqiqə

        [Range(0, 60)]
        public int MaxLatenessMinutes { get; set; } = 15;

        //[Range(0, 60)]
        //public int MaxEarlyLeaveMinutes { get; set; } = 15;

        public bool IsActive { get; set; } = true;
    }

}
