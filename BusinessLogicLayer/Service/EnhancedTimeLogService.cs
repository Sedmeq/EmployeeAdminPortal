using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using Models.Models.Dto;
using Models.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public class EnhancedTimeLogService : ITimeLogService
    {
        private readonly ApplicationDbContext _context;
        private const string DateTimeFormat = "dd.MM.yyyy HH:mm:ss";
        private const string DateFormat = "dd.MM.yyyy";
        private const string TimeFormat = "HH:mm:ss";

        public EnhancedTimeLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TimeLogDto?> CheckInAsync(Guid employeeId, CheckInDto checkInDto)
        {
            // Employee və onun work schedule məlumatını əldə et
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Include(e => e.WorkSchedule)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return null;

            // Aktiv sessiya olub olmadığını yoxla
            var activeSession = await _context.EmployeeTimeLogs
                .Where(t => t.EmployeeId == employeeId && t.CheckOutTime == null)
                .FirstOrDefaultAsync();

            if (activeSession != null)
                return null;

            var checkInTime = DateTime.Now;
            var timeLog = new EmployeeTimeLog
            {
                EmployeeId = employeeId,
                CheckInTime = checkInTime,
                Notes = checkInDto.Notes
            };

            _context.EmployeeTimeLogs.Add(timeLog);
            await _context.SaveChangesAsync();

            return ConvertToEnhancedTimeLogDto(timeLog, employee);
        }

        public async Task<TimeLogDto?> CheckOutAsync(Guid employeeId, CheckOutDto checkOutDto)
        {
            var activeSession = await _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .Where(t => t.EmployeeId == employeeId && t.CheckOutTime == null)
                .FirstOrDefaultAsync();

            if (activeSession == null)
                return null;

            activeSession.CheckOutTime = DateTime.Now;
            activeSession.CalculateWorkDuration();

            // Notes-ları yeniləmək
            if (!string.IsNullOrEmpty(checkOutDto.Notes))
            {
                activeSession.Notes = string.IsNullOrEmpty(activeSession.Notes)
                    ? checkOutDto.Notes
                    : $"{activeSession.Notes} | Checkout: {checkOutDto.Notes}";
            }

            await _context.SaveChangesAsync();

            return ConvertToEnhancedTimeLogDto(activeSession, activeSession.Employee);
        }

        public async Task<TimeLogDto?> GetActiveSessionAsync(Guid employeeId)
        {
            var activeSession = await _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .Where(t => t.EmployeeId == employeeId && t.CheckOutTime == null)
                .FirstOrDefaultAsync();

            if (activeSession == null)
                return null;

            return ConvertToEnhancedTimeLogDto(activeSession, activeSession.Employee);
        }

        public async Task<List<TimeLogDto>> GetEmployeeTimeLogsAsync(Guid employeeId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .Where(t => t.EmployeeId == employeeId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CheckInTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CheckInTime <= toDate.Value.AddDays(1));

            var timeLogs = await query
                .OrderByDescending(t => t.CheckInTime)
                .ToListAsync();

            return timeLogs
    .Select(t => (TimeLogDto)ConvertToEnhancedTimeLogDto(t, t.Employee))
    .ToList();

        }

        public async Task<List<TimeLogDto>> GetAllTimeLogsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(t => t.CheckInTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CheckInTime <= toDate.Value.AddDays(1));

            var timeLogs = await query
                .OrderByDescending(t => t.CheckInTime)
                .ToListAsync();

            return timeLogs
    .Select(t => (TimeLogDto)ConvertToEnhancedTimeLogDto(t, t.Employee))
    .ToList();

        }

        public async Task<List<TimeLogDto>> GetTimeLogsByDepartmentAsync(Guid departmentId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .Where(t => t.Employee.DepartmentId == departmentId);

            if (fromDate.HasValue)
                query = query.Where(t => t.CheckInTime >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CheckInTime <= toDate.Value.AddDays(1));

            var timeLogs = await query
                .OrderByDescending(t => t.CheckInTime)
                .ToListAsync();

            return timeLogs
    .Select(t => (TimeLogDto)ConvertToEnhancedTimeLogDto(t, t.Employee))
    .ToList();

        }

        public async Task<DailyTimeLogSummaryDto> GetDailyTimeLogSummaryAsync(Guid employeeId, DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            var dailyLogs = await _context.EmployeeTimeLogs
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Department)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.Role)
                .Include(t => t.Employee)
                    .ThenInclude(e => e.WorkSchedule)
                .Where(t => t.EmployeeId == employeeId &&
                           t.CheckInTime >= startDate &&
                           t.CheckInTime < endDate)
                .OrderBy(t => t.CheckInTime)
                .ToListAsync();

            var totalWorkTime = dailyLogs
                .Where(t => t.WorkDuration.HasValue)
                .Sum(t => t.WorkDuration.Value.Ticks);

            var totalTimeSpan = new TimeSpan(totalWorkTime);

            return new DailyTimeLogSummaryDto
            {
                Date = date.ToString(DateFormat),
                DateRaw = date.Date,
                TotalWorkTime = FormatTimeSpan(totalTimeSpan),
                TotalWorkTimeRaw = totalTimeSpan,
                CheckInCount = dailyLogs.Count,
                TimeLogs = dailyLogs
    .Select(t => (TimeLogDto)ConvertToEnhancedTimeLogDto(t, t.Employee))
    .ToList()
            };
        }

        public async Task<List<TimeLogDto>> GetTimeLogsByDateRangeAsync(Guid employeeId, DateTime fromDate, DateTime toDate)
        {
            return await GetEmployeeTimeLogsAsync(employeeId, fromDate, toDate);
        }

        public async Task<bool> IsEmployeeCheckedInAsync(Guid employeeId)
        {
            return await _context.EmployeeTimeLogs
                .AnyAsync(t => t.EmployeeId == employeeId && t.CheckOutTime == null);
        }

        public async Task<TimeSpan> GetTotalWorkTimeAsync(Guid employeeId, DateTime fromDate, DateTime toDate)
        {
            var timeLogs = await _context.EmployeeTimeLogs
                .Where(t => t.EmployeeId == employeeId &&
                           t.CheckInTime >= fromDate &&
                           t.CheckInTime <= toDate.AddDays(1) &&
                           t.WorkDuration.HasValue)
                .ToListAsync();

            var totalTicks = timeLogs.Sum(t => t.WorkDuration.Value.Ticks);
            return new TimeSpan(totalTicks);
        }

        private EnhancedTimeLogDto ConvertToEnhancedTimeLogDto(EmployeeTimeLog timeLog, EmployeeAdminPortal.Models.Entities.Employee employee)
        {
            var basicDto = new EnhancedTimeLogDto
            {
                Id = timeLog.Id,
                EmployeeId = timeLog.EmployeeId,
                EmployeeName = employee.Username,
                DepartmentName = employee.Department?.Name,
                RoleName = employee.Role?.Name,
                CheckInTime = timeLog.CheckInTime.ToString(DateTimeFormat),
                CheckInTimeRaw = timeLog.CheckInTime,
                CheckOutTime = timeLog.CheckOutTime?.ToString(DateTimeFormat),
                CheckOutTimeRaw = timeLog.CheckOutTime,
                WorkDuration = timeLog.WorkDuration.HasValue ? FormatTimeSpan(timeLog.WorkDuration.Value) : null,
                WorkDurationRaw = timeLog.WorkDuration,
                WorkDurationInMinutes = timeLog.WorkDuration.HasValue ? (int)timeLog.WorkDuration.Value.TotalMinutes : null,
                Notes = timeLog.Notes,
                IsCheckedOut = timeLog.IsCheckedOut
            };

            // Work Schedule analizi əlavə et
            if (employee.WorkSchedule != null)
            {
                var schedule = employee.WorkSchedule;
                var checkInTime = timeLog.CheckInTime.TimeOfDay;
                var checkOutTime = timeLog.CheckOutTime?.TimeOfDay;

                basicDto.WorkScheduleName = schedule.Name;
                basicDto.ExpectedStartTime = schedule.StartTime.ToString(@"hh\:mm");
                basicDto.ExpectedEndTime = schedule.EndTime.ToString(@"hh\:mm");

                // Gecikmə hesabla
                if (checkInTime > schedule.StartTime)
                {
                    var lateness = checkInTime - schedule.StartTime;
                    basicDto.LatenessTime = FormatTimeSpan(lateness);
                    basicDto.IsLate = lateness > TimeSpan.FromMinutes(schedule.MaxLatenessMinutes);
                }
                else
                {
                    basicDto.LatenessTime = "00:00:00";
                    basicDto.IsLate = false;
                }

                // Erkən gedişi hesabla
                if (checkOutTime.HasValue && checkOutTime.Value < schedule.EndTime)
                {
                    var earlyLeave = schedule.EndTime - checkOutTime.Value;
                    basicDto.EarlyLeaveTime = FormatTimeSpan(earlyLeave);
                    basicDto.IsEarlyLeave = earlyLeave > TimeSpan.FromMinutes(schedule.MaxEarlyLeaveMinutes);
                }
                else
                {
                    basicDto.EarlyLeaveTime = checkOutTime.HasValue ? "00:00:00" : null;
                    basicDto.IsEarlyLeave = false;
                }

                // İş saatları daxilində olub olmadığını yoxla
                basicDto.IsWithinSchedule = schedule.IsWithinWorkHours(checkInTime);

                // İş effektivliyi hesabla
                if (timeLog.WorkDuration.HasValue)
                {
                    var expectedWorkTime = schedule.EndTime - schedule.StartTime;
                    var efficiency = (timeLog.WorkDuration.Value.TotalMinutes / expectedWorkTime.TotalMinutes) * 100;
                    basicDto.WorkEfficiency = $"{efficiency:F1}%";
                }
            }

            return basicDto;
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return $"{(int)timeSpan.TotalDays} gün, {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}