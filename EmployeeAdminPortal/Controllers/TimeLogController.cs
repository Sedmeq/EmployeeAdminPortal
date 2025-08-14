using BusinessLogicLayer.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models.Dto;
using System.Security.Claims;

namespace EmployeeAdminPortal.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TimeLogController : ControllerBase
    {
        private readonly ITimeLogService _timeLogService;

        public TimeLogController(ITimeLogService timeLogService)
        {
            _timeLogService = timeLogService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto checkInDto)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.CheckInAsync(employeeId.Value, checkInDto);

            if (result == null)
                return BadRequest("Check-in failed. You may already be checked in.");

            return Ok(new { message = "Checked in successfully", data = result });
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] CheckOutDto checkOutDto)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.CheckOutAsync(employeeId.Value, checkOutDto);

            if (result == null)
                return BadRequest("Check-out failed. You may not be checked in.");

            return Ok(new { message = "Checked out successfully", data = result });
        }

        [HttpGet("active-session")]
        public async Task<IActionResult> GetActiveSession()
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.GetActiveSessionAsync(employeeId.Value);

            if (result == null)
                return Ok(new { message = "No active session", data = (object)null });

            return Ok(new { message = "Active session found", data = result });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var isCheckedIn = await _timeLogService.IsEmployeeCheckedInAsync(employeeId.Value);
            var activeSession = await _timeLogService.GetActiveSessionAsync(employeeId.Value);

            return Ok(new
            {
                isCheckedIn = isCheckedIn,
                activeSession = activeSession,
                currentTime = DateTime.Now
            });
        }

        [HttpGet("my-logs")]
        public async Task<IActionResult> GetMyTimeLogs(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.GetEmployeeTimeLogsAsync(
                employeeId.Value, fromDate, toDate);

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("my-summary/{date}")]
        public async Task<IActionResult> GetDailySummary(DateTime date)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.GetDailyTimeLogSummaryAsync(employeeId.Value, date);

            return Ok(new { data = result });
        }

        [HttpGet("my-total-work-time")]
        public async Task<IActionResult> GetTotalWorkTime(
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var totalTime = await _timeLogService.GetTotalWorkTimeAsync(employeeId.Value, fromDate, toDate);

            return Ok(new
            {
                fromDate = fromDate,
                toDate = toDate,
                totalWorkTime = totalTime.ToString(@"hh\:mm\:ss"),
                totalHours = totalTime.TotalHours,
                totalDays = totalTime.TotalDays
            });
        }

        // Admin endpoints
        [HttpGet("all-logs")]
        public async Task<IActionResult> GetAllTimeLogs(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            // In a real application, you should check if the current user is an admin
            var result = await _timeLogService.GetAllTimeLogsAsync(fromDate, toDate);

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("employee/{employeeId}/logs")]
        public async Task<IActionResult> GetEmployeeTimeLogs(
            Guid employeeId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            // In a real application, you should check if the current user is an admin
            var result = await _timeLogService.GetEmployeeTimeLogsAsync(employeeId, fromDate, toDate);

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("employee/{employeeId}/summary/{date}")]
        public async Task<IActionResult> GetEmployeeDailySummary(Guid employeeId, DateTime date)
        {
            // In a real application, you should check if the current user is an admin
            var result = await _timeLogService.GetDailyTimeLogSummaryAsync(employeeId, date);

            return Ok(new { data = result });
        }

        [HttpGet("employee/{employeeId}/status")]
        public async Task<IActionResult> GetEmployeeStatus(Guid employeeId)
        {
            // In a real application, you should check if the current user is an admin
            var isCheckedIn = await _timeLogService.IsEmployeeCheckedInAsync(employeeId);
            var activeSession = await _timeLogService.GetActiveSessionAsync(employeeId);

            return Ok(new
            {
                employeeId = employeeId,
                isCheckedIn = isCheckedIn,
                activeSession = activeSession,
                currentTime = DateTime.Now
            });
        }

        private Guid? GetCurrentEmployeeId()
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (employeeIdClaim != null && Guid.TryParse(employeeIdClaim, out var employeeId))
            {
                return employeeId;
            }
            return null;
        }
    }
}
