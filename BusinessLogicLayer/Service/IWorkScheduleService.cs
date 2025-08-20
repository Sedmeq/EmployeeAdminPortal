using Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public interface IWorkScheduleService
    {
        Task<List<WorkScheduleResponseDto>> GetAllWorkSchedulesAsync();
        Task<WorkScheduleResponseDto?> GetWorkScheduleByIdAsync(Guid id);
        Task<WorkScheduleResponseDto?> CreateWorkScheduleAsync(WorkScheduleDto workScheduleDto);
        Task<WorkScheduleResponseDto?> UpdateWorkScheduleAsync(Guid id, WorkScheduleDto workScheduleDto);
        Task<bool> DeleteWorkScheduleAsync(Guid id);
        Task<bool> WorkScheduleExistsAsync(Guid id);
        Task<List<WorkScheduleResponseDto>> GetActiveWorkSchedulesAsync();

        // İş saatı analizi metodları
        //Task<bool> IsWorkingTimeAsync(Guid workScheduleId, TimeSpan time);
        //Task<bool> IsLateAsync(Guid workScheduleId, TimeSpan checkInTime);
        //Task<bool> IsEarlyLeaveAsync(Guid workScheduleId, TimeSpan checkOutTime);
        //Task<TimeSpan> GetLatenessTimeAsync(Guid workScheduleId, TimeSpan checkInTime);
        //Task<TimeSpan> GetEarlyLeaveTimeAsync(Guid workScheduleId, TimeSpan checkOutTime);
    }
}
