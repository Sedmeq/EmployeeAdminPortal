using BusinessLogicLayer.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models.Dto;

namespace EmployeeAdminPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleService _workScheduleService;

        public WorkScheduleController(IWorkScheduleService workScheduleService)
        {
            _workScheduleService = workScheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _workScheduleService.GetAllWorkSchedulesAsync();
            return Ok(new { data = schedules, count = schedules.Count });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var schedules = await _workScheduleService.GetActiveWorkSchedulesAsync();
            return Ok(new { data = schedules, count = schedules.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var schedule = await _workScheduleService.GetWorkScheduleByIdAsync(id);
            if (schedule == null)
                return NotFound(new { message = "Work schedule not found" });

            return Ok(new { data = schedule });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkScheduleDto workScheduleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _workScheduleService.CreateWorkScheduleAsync(workScheduleDto);
            if (created == null)
                return BadRequest(new { message = "Work schedule name already exists or invalid time format" });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WorkScheduleDto workScheduleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _workScheduleService.UpdateWorkScheduleAsync(id, workScheduleDto);
            if (updated == null)
                return NotFound(new { message = "Work schedule not found, name already exists, or invalid time format" });

            return Ok(new { data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _workScheduleService.DeleteWorkScheduleAsync(id);
            if (!deleted)
                return BadRequest(new { message = "Work schedule not found or is being used by employees" });

            return Ok(new { message = "Work schedule deleted successfully" });
        }

        [HttpGet("{id}/validate-time")]
        public async Task<IActionResult> ValidateTime(Guid id, [FromQuery] string time)
        {
            if (!TimeSpan.TryParse(time, out TimeSpan timeSpan))
                return BadRequest(new { message = "Invalid time format. Use HH:mm format" });

            var isWorkingTime = await _workScheduleService.IsWorkingTimeAsync(id, timeSpan);
            var isLate = await _workScheduleService.IsLateAsync(id, timeSpan);
            var latenessTime = await _workScheduleService.GetLatenessTimeAsync(id, timeSpan);

            return Ok(new
            {
                time = time,
                isWorkingTime = isWorkingTime,
                isLate = isLate,
                latenessMinutes = latenessTime.TotalMinutes,
                latenessTime = latenessTime.ToString(@"hh\:mm\:ss")
            });
        }
    }
}