using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Dto
{
    public class DailyTimeLogSummaryDto
    {
        public DateTime Date { get; set; }
        public TimeSpan TotalWorkTime { get; set; }
        public int CheckInCount { get; set; }
        public List<TimeLogDto> TimeLogs { get; set; } = new List<TimeLogDto>();
    }
}
