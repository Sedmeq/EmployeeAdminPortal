using BusinessLogicLayer.Service;
using EmployeeAdminPortal.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models.Dto;
using System.Security.Claims;

namespace EmployeeAdminPortal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            List<EmployeeResponseDto> employees;

            // Boss can see all employees
            if (HasAdminAccess(currentEmployee))
            {
                employees = await _employeeService.GetAllEmployeesAsync();
            }
            // Department bosses can see employees from their department
            else if (HasDepartmentBossAccess(currentEmployee) && currentEmployee.DepartmentId.HasValue)
            {
                employees = await _employeeService.GetEmployeesByDepartmentAsync(currentEmployee.DepartmentId.Value);
            }
            // Regular employees can only see themselves
            else
            {
                var selfEmployee = await _employeeService.GetEmployeeByIdAsync(currentEmployee.Id);
                employees = selfEmployee != null ? new List<EmployeeResponseDto> { selfEmployee } : new List<EmployeeResponseDto>();
            }

            return Ok(new { data = employees, count = employees.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            var targetEmployee = await _employeeService.GetEmployeeEntityByIdAsync(id);
            if (targetEmployee == null)
                return NotFound(new { message = "Employee not found" });

            // Check access permissions
            if (!CanAccessEmployeeData(currentEmployee, targetEmployee))
                return Forbid("Access denied. You can only view employees from your department.");

            var employeeDto = await _employeeService.GetEmployeeByIdAsync(id);
            return Ok(new { data = employeeDto });
        }

        //[HttpGet("my-profile")]
        //public async Task<IActionResult> GetMyProfile()
        //{
        //    var employeeId = GetCurrentEmployeeId();
        //    if (employeeId == null)
        //        return Unauthorized("Employee ID not found in token");

        //    var employee = await _employeeService.GetEmployeeByIdAsync(employeeId.Value);
        //    if (employee == null)
        //        return NotFound(new { message = "Employee not found" });

        //    return Ok(new { data = employee });
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeDto employeeDto)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            // Only Boss can create new employees
            if (!HasAdminAccess(currentEmployee))
                return Forbid("Access denied. Admin role required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _employeeService.AddEmployeeAsync(employeeDto);
            if (created == null)
                return BadRequest(new { message = "Email already exists" });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] EmployeeDto employeeDto)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            var targetEmployee = await _employeeService.GetEmployeeEntityByIdAsync(id);
            if (targetEmployee == null)
                return NotFound(new { message = "Employee not found" });

            // Check permissions: Boss can update anyone, Department bosses can update their department employees, employees can update themselves
            if (!CanModifyEmployeeData(currentEmployee, targetEmployee))
                return Forbid("Access denied. You don't have permission to modify this employee.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
            if (updated == null)
                return BadRequest(new { message = "Email already exists or employee not found" });

            return Ok(new { data = updated });
        }

        //[HttpPut("my-profile")]
        //public async Task<IActionResult> UpdateMyProfile([FromBody] EmployeeDto employeeDto)
        //{
        //    var employeeId = GetCurrentEmployeeId();
        //    if (employeeId == null)
        //        return Unauthorized("Employee ID not found in token");

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var updated = await _employeeService.UpdateEmployeeAsync(employeeId.Value, employeeDto);
        //    if (updated == null)
        //        return BadRequest(new { message = "Email already exists or employee not found" });

        //    return Ok(new { data = updated });
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var currentEmployee = await GetCurrentEmployeeAsync();
            if (currentEmployee == null)
                return Unauthorized("Employee not found");

            // Only Boss can delete employees
            if (!HasAdminAccess(currentEmployee))
                return Forbid("Access denied. Admin role required.");

            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (!deleted)
                return NotFound(new { message = "Employee not found" });

            return Ok(new { message = "Employee deleted successfully" });
        }

        //[HttpGet("department/{departmentId}")]
        //public async Task<IActionResult> GetByDepartment(Guid departmentId)
        //{
        //    var currentEmployee = await GetCurrentEmployeeAsync();
        //    if (currentEmployee == null)
        //        return Unauthorized("Employee not found");

        //    // Check permissions
        //    if (!HasAdminAccess(currentEmployee))
        //    {
        //        // Department bosses can only see their own department
        //        if (!HasDepartmentBossAccess(currentEmployee) || currentEmployee.DepartmentId != departmentId)
        //            return Forbid("Access denied. You can only view employees from your department.");
        //    }

        //    var employees = await _employeeService.GetEmployeesByDepartmentAsync(departmentId);
        //    return Ok(new { data = employees, count = employees.Count });
        //}

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

        private bool CanModifyEmployeeData(EmployeeAdminPortal.Models.Entities.Employee currentEmployee, EmployeeAdminPortal.Models.Entities.Employee targetEmployee)
        {
            // Boss can modify everyone
            if (HasAdminAccess(currentEmployee))
                return true;

            // Department bosses can modify employees from their department (except other bosses)
            if (HasDepartmentBossAccess(currentEmployee))
            {
                var targetIsNotBoss = targetEmployee.Role?.Name == "Employee";
                return currentEmployee.DepartmentId != null &&
                       currentEmployee.DepartmentId == targetEmployee.DepartmentId &&
                       targetIsNotBoss;
            }

            // Employees can only modify their own data
            return currentEmployee.Id == targetEmployee.Id;
        }

        #endregion
    }
}