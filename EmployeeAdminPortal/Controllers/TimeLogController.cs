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
        private readonly IEmployeeService _employeeService;

        public TimeLogController(ITimeLogService timeLogService, IEmployeeService employeeService)
        {
            _timeLogService = timeLogService;
            _employeeService = employeeService;
        }

        #region Employee Operations

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] CheckInDto checkInDto)
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return Unauthorized("Employee ID not found in token");

            var result = await _timeLogService.CheckInAsync(employeeId.Value, checkInDto);

            if (result == null)
                return BadRequest(new { message = "Check-in failed. You may already be checked in." });

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
                return BadRequest(new { message = "Check-out failed. You may not be checked in." });

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
                currentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
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
                fromDate = fromDate.ToString("dd.MM.yyyy"),
                toDate = toDate.ToString("dd.MM.yyyy"),
                totalWorkTime = FormatTimeSpan(totalTime),
                totalHours = Math.Round(totalTime.TotalHours, 2),
                totalDays = Math.Round(totalTime.TotalDays, 2)
            });
        }

        #endregion

        #region Admin/Manager Operations

        [HttpGet("all-logs")]
        public async Task<IActionResult> GetAllTimeLogs(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            // Check if user has admin access
            if (!HasAdminAccess(currentEmployee))
                return Forbid("Access denied. Admin role required.");

            var result = await _timeLogService.GetAllTimeLogsAsync(fromDate, toDate);

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("department-logs")]
        public async Task<IActionResult> GetDepartmentTimeLogs(
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            // Check if user has department boss access
            if (!HasDepartmentBossAccess(currentEmployee) && !HasAdminAccess(currentEmployee))
                return Forbid("Access denied. Department manager role required.");

            List<TimeLogDto> result;

            if (HasAdminAccess(currentEmployee))
            {
                // Boss can see all departments
                result = await _timeLogService.GetAllTimeLogsAsync(fromDate, toDate);
            }
            else
            {
                // Department boss can only see their department
                if (currentEmployee.DepartmentId == null)
                    return BadRequest("Department not assigned to your account");

                result = await _timeLogService.GetTimeLogsByDepartmentAsync(
                    currentEmployee.DepartmentId.Value, fromDate, toDate);
            }

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("employee/{employeeId}/logs")]
        public async Task<IActionResult> GetEmployeeTimeLogs(
            Guid employeeId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            var targetEmployee = await _employeeService.GetEmployeeEntityByIdAsync(employeeId);
            if (targetEmployee == null)
                return NotFound("Target employee not found");

            // Check access permissions
            if (!CanAccessEmployeeData(currentEmployee, targetEmployee))
                return Forbid("Access denied. You can only view employees from your department.");

            var result = await _timeLogService.GetEmployeeTimeLogsAsync(employeeId, fromDate, toDate);

            return Ok(new { data = result, count = result.Count });
        }

        [HttpGet("employee/{employeeId}/summary/{date}")]
        public async Task<IActionResult> GetEmployeeDailySummary(Guid employeeId, DateTime date)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            var targetEmployee = await _employeeService.GetEmployeeEntityByIdAsync(employeeId);
            if (targetEmployee == null)
                return NotFound("Target employee not found");

            // Check access permissions
            if (!CanAccessEmployeeData(currentEmployee, targetEmployee))
                return Forbid("Access denied. You can only view employees from your department.");

            var result = await _timeLogService.GetDailyTimeLogSummaryAsync(employeeId, date);

            return Ok(new { data = result });
        }

        [HttpGet("employee/{employeeId}/status")]
        public async Task<IActionResult> GetEmployeeStatus(Guid employeeId)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            var targetEmployee = await _employeeService.GetEmployeeEntityByIdAsync(employeeId);
            if (targetEmployee == null)
                return NotFound("Target employee not found");

            // Check access permissions
            if (!CanAccessEmployeeData(currentEmployee, targetEmployee))
                return Forbid("Access denied. You can only view employees from your department.");

            var isCheckedIn = await _timeLogService.IsEmployeeCheckedInAsync(employeeId);
            var activeSession = await _timeLogService.GetActiveSessionAsync(employeeId);

            return Ok(new
            {
                employeeId = employeeId,
                employeeName = targetEmployee.Username,
                departmentName = targetEmployee.Department?.Name,
                isCheckedIn = isCheckedIn,
                activeSession = activeSession,
                currentTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
            });
        }

        #endregion

        #region Helper Methods

        private Guid? GetCurrentEmployeeId()
        {
            var employeeIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (employeeIdClaim != null && Guid.TryParse(employeeIdClaim, out var employeeId))
            {
                return employeeId;
            }
            return null;
        }

        private async Task<EmployeeAdminPortal.Models.Entities.Employee?> GetCurrentEmployeeAsync()
        {
            var employeeId = GetCurrentEmployeeId();
            if (employeeId == null)
                return null;

            return await _employeeService.GetEmployeeEntityByIdAsync(employeeId.Value);
        }

        private bool HasAdminAccess(EmployeeAdminPortal.Models.Entities.Employee employee)
        {
            return employee.Role?.Name == "Boss";
        }

        private bool HasDepartmentBossAccess(EmployeeAdminPortal.Models.Entities.Employee employee)
        {
            var roleName = employee.Role?.Name;
            return roleName != null && (roleName.StartsWith("Boss-") || roleName == "Boss");
        }

        private bool CanAccessEmployeeData(EmployeeAdminPortal.Models.Entities.Employee currentEmployee, EmployeeAdminPortal.Models.Entities.Employee targetEmployee)
        {
            // Boss can access everyone
            if (HasAdminAccess(currentEmployee))
                return true;

            // Department bosses can access employees from their department
            if (HasDepartmentBossAccess(currentEmployee))
            {
                return currentEmployee.DepartmentId != null &&
                       currentEmployee.DepartmentId == targetEmployee.DepartmentId;
            }

            // Employees can only access their own data
            return currentEmployee.Id == targetEmployee.Id;
        }

        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            if (timeSpan.TotalDays >= 1)
            {
                return $"{(int)timeSpan.TotalDays} gün, {timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        #endregion
    }
}