using Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public interface ITimeLogService
    {
        // Check-in/Check-out operations
        Task<TimeLogDto?> CheckInAsync(Guid employeeId, CheckInDto checkInDto);
        Task<TimeLogDto?> CheckOutAsync(Guid employeeId, CheckOutDto checkOutDto);

        // Get current active session
        Task<TimeLogDto?> GetActiveSessionAsync(Guid employeeId);

        // Get employee time logs
        Task<List<TimeLogDto>> GetEmployeeTimeLogsAsync(Guid employeeId, DateTime? fromDate = null, DateTime? toDate = null);

        // Get all employees time logs (for admin)
        Task<List<TimeLogDto>> GetAllTimeLogsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        // Get time logs by department (for department bosses)
        Task<List<TimeLogDto>> GetTimeLogsByDepartmentAsync(Guid departmentId, DateTime? fromDate = null, DateTime? toDate = null);

        // Get daily summary
        Task<DailyTimeLogSummaryDto> GetDailyTimeLogSummaryAsync(Guid employeeId, DateTime date);

        // Get time logs for a specific date range
        Task<List<TimeLogDto>> GetTimeLogsByDateRangeAsync(Guid employeeId, DateTime fromDate, DateTime toDate);

        // Check if employee is currently checked in
        Task<bool> IsEmployeeCheckedInAsync(Guid employeeId);

        // Get total work time for a period
        Task<TimeSpan> GetTotalWorkTimeAsync(Guid employeeId, DateTime fromDate, DateTime toDate);
    }
}