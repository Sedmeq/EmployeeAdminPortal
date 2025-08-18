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
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly ApplicationDbContext _context;

        public WorkScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkScheduleResponseDto>> GetAllWorkSchedulesAsync()
        {
            return await _context.WorkSchedules
                .Include(ws => ws.Employees)
                .Select(ws => new WorkScheduleResponseDto
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    Description = ws.Description,
                    StartTime = ws.StartTime.ToString(@"hh\:mm"),
                    EndTime = ws.EndTime.ToString(@"hh\:mm"),
                    RequiredWorkHours = ws.RequiredWorkHours,
                    MinimumWorkMinutes = ws.MinimumWorkMinutes,
                    MaxLatenessMinutes = ws.MaxLatenessMinutes,
                    MaxEarlyLeaveMinutes = ws.MaxEarlyLeaveMinutes,
                    CreatedAt = ws.CreatedAt,
                    IsActive = ws.IsActive,
                    EmployeeCount = ws.Employees.Count
                })
                .ToListAsync();
        }

        public async Task<WorkScheduleResponseDto?> GetWorkScheduleByIdAsync(Guid id)
        {
            var workSchedule = await _context.WorkSchedules
                .Include(ws => ws.Employees)
                .FirstOrDefaultAsync(ws => ws.Id == id);

            if (workSchedule == null)
                return null;

            return new WorkScheduleResponseDto
            {
                Id = workSchedule.Id,
                Name = workSchedule.Name,
                Description = workSchedule.Description,
                StartTime = workSchedule.StartTime.ToString(@"hh\:mm"),
                EndTime = workSchedule.EndTime.ToString(@"hh\:mm"),
                RequiredWorkHours = workSchedule.RequiredWorkHours,
                MinimumWorkMinutes = workSchedule.MinimumWorkMinutes,
                MaxLatenessMinutes = workSchedule.MaxLatenessMinutes,
                MaxEarlyLeaveMinutes = workSchedule.MaxEarlyLeaveMinutes,
                CreatedAt = workSchedule.CreatedAt,
                IsActive = workSchedule.IsActive,
                EmployeeCount = workSchedule.Employees.Count
            };
        }

        public async Task<WorkScheduleResponseDto?> CreateWorkScheduleAsync(WorkScheduleDto workScheduleDto)
        {
            // Check if schedule name already exists
            if (await _context.WorkSchedules.AnyAsync(ws => ws.Name.ToLower() == workScheduleDto.Name.ToLower()))
                return null;

            // Parse time strings
            if (!TimeSpan.TryParse(workScheduleDto.StartTime, out TimeSpan startTime) ||
                !TimeSpan.TryParse(workScheduleDto.EndTime, out TimeSpan endTime))
                return null;

            var workSchedule = new WorkSchedule
            {
                Id = Guid.NewGuid(),
                Name = workScheduleDto.Name,
                Description = workScheduleDto.Description,
                StartTime = startTime,
                EndTime = endTime,
                RequiredWorkHours = workScheduleDto.RequiredWorkHours,
                MinimumWorkMinutes = workScheduleDto.MinimumWorkMinutes,
                MaxLatenessMinutes = workScheduleDto.MaxLatenessMinutes,
                MaxEarlyLeaveMinutes = workScheduleDto.MaxEarlyLeaveMinutes,
                IsActive = workScheduleDto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.WorkSchedules.Add(workSchedule);
            await _context.SaveChangesAsync();

            return new WorkScheduleResponseDto
            {
                Id = workSchedule.Id,
                Name = workSchedule.Name,
                Description = workSchedule.Description,
                StartTime = workSchedule.StartTime.ToString(@"hh\:mm"),
                EndTime = workSchedule.EndTime.ToString(@"hh\:mm"),
                RequiredWorkHours = workSchedule.RequiredWorkHours,
                MinimumWorkMinutes = workSchedule.MinimumWorkMinutes,
                MaxLatenessMinutes = workSchedule.MaxLatenessMinutes,
                MaxEarlyLeaveMinutes = workSchedule.MaxEarlyLeaveMinutes,
                CreatedAt = workSchedule.CreatedAt,
                IsActive = workSchedule.IsActive,
                EmployeeCount = 0
            };
        }

        public async Task<WorkScheduleResponseDto?> UpdateWorkScheduleAsync(Guid id, WorkScheduleDto workScheduleDto)
        {
            var workSchedule = await _context.WorkSchedules
                .Include(ws => ws.Employees)
                .FirstOrDefaultAsync(ws => ws.Id == id);

            if (workSchedule == null)
                return null;

            // Check if new name conflicts with existing schedule
            if (await _context.WorkSchedules.AnyAsync(ws => ws.Id != id && ws.Name.ToLower() == workScheduleDto.Name.ToLower()))
                return null;

            // Parse time strings
            if (!TimeSpan.TryParse(workScheduleDto.StartTime, out TimeSpan startTime) ||
                !TimeSpan.TryParse(workScheduleDto.EndTime, out TimeSpan endTime))
                return null;

            workSchedule.Name = workScheduleDto.Name;
            workSchedule.Description = workScheduleDto.Description;
            workSchedule.StartTime = startTime;
            workSchedule.EndTime = endTime;
            workSchedule.RequiredWorkHours = workScheduleDto.RequiredWorkHours;
            workSchedule.MinimumWorkMinutes = workScheduleDto.MinimumWorkMinutes;
            workSchedule.MaxLatenessMinutes = workScheduleDto.MaxLatenessMinutes;
            workSchedule.MaxEarlyLeaveMinutes = workScheduleDto.MaxEarlyLeaveMinutes;
            workSchedule.IsActive = workScheduleDto.IsActive;

            await _context.SaveChangesAsync();

            return new WorkScheduleResponseDto
            {
                Id = workSchedule.Id,
                Name = workSchedule.Name,
                Description = workSchedule.Description,
                StartTime = workSchedule.StartTime.ToString(@"hh\:mm"),
                EndTime = workSchedule.EndTime.ToString(@"hh\:mm"),
                RequiredWorkHours = workSchedule.RequiredWorkHours,
                MinimumWorkMinutes = workSchedule.MinimumWorkMinutes,
                MaxLatenessMinutes = workSchedule.MaxLatenessMinutes,
                MaxEarlyLeaveMinutes = workSchedule.MaxEarlyLeaveMinutes,
                CreatedAt = workSchedule.CreatedAt,
                IsActive = workSchedule.IsActive,
                EmployeeCount = workSchedule.Employees.Count
            };
        }

        public async Task<bool> DeleteWorkScheduleAsync(Guid id)
        {
            var workSchedule = await _context.WorkSchedules.FindAsync(id);
            if (workSchedule == null)
                return false;

            // Check if any employees are using this schedule
            var hasEmployees = await _context.Employees.AnyAsync(e => e.WorkScheduleId == id);
            if (hasEmployees)
                return false; // Cannot delete if employees are using it

            _context.WorkSchedules.Remove(workSchedule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WorkScheduleExistsAsync(Guid id)
        {
            return await _context.WorkSchedules.AnyAsync(ws => ws.Id == id);
        }

        public async Task<List<WorkScheduleResponseDto>> GetActiveWorkSchedulesAsync()
        {
            return await _context.WorkSchedules
                .Where(ws => ws.IsActive)
                .Include(ws => ws.Employees)
                .Select(ws => new WorkScheduleResponseDto
                {
                    Id = ws.Id,
                    Name = ws.Name,
                    Description = ws.Description,
                    StartTime = ws.StartTime.ToString(@"hh\:mm"),
                    EndTime = ws.EndTime.ToString(@"hh\:mm"),
                    RequiredWorkHours = ws.RequiredWorkHours,
                    MinimumWorkMinutes = ws.MinimumWorkMinutes,
                    MaxLatenessMinutes = ws.MaxLatenessMinutes,
                    MaxEarlyLeaveMinutes = ws.MaxEarlyLeaveMinutes,
                    CreatedAt = ws.CreatedAt,
                    IsActive = ws.IsActive,
                    EmployeeCount = ws.Employees.Count
                })
                .ToListAsync();
        }

        public async Task<bool> IsWorkingTimeAsync(Guid workScheduleId, TimeSpan time)
        {
            var schedule = await _context.WorkSchedules.FindAsync(workScheduleId);
            return schedule?.IsWithinWorkHours(time) ?? false;
        }

        public async Task<bool> IsLateAsync(Guid workScheduleId, TimeSpan checkInTime)
        {
            var schedule = await _context.WorkSchedules.FindAsync(workScheduleId);
            return schedule?.IsLate(checkInTime) ?? false;
        }

        public async Task<bool> IsEarlyLeaveAsync(Guid workScheduleId, TimeSpan checkOutTime)
        {
            var schedule = await _context.WorkSchedules.FindAsync(workScheduleId);
            return schedule?.IsEarlyLeave(checkOutTime) ?? false;
        }

        public async Task<TimeSpan> GetLatenessTimeAsync(Guid workScheduleId, TimeSpan checkInTime)
        {
            var schedule = await _context.WorkSchedules.FindAsync(workScheduleId);
            return schedule?.GetLatenessTime(checkInTime) ?? TimeSpan.Zero;
        }

        public async Task<TimeSpan> GetEarlyLeaveTimeAsync(Guid workScheduleId, TimeSpan checkOutTime)
        {
            var schedule = await _context.WorkSchedules.FindAsync(workScheduleId);
            return schedule?.GetEarlyLeaveTime(checkOutTime) ?? TimeSpan.Zero;
        }
    }
}
