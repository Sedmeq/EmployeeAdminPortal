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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return Ok(new { data = departments, count = departments.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            return Ok(new { data = department });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _departmentService.CreateDepartmentAsync(departmentDto);
            if (created == null)
                return BadRequest(new { message = "Department name already exists" });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] DepartmentDto departmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _departmentService.UpdateDepartmentAsync(id, departmentDto);
            if (updated == null)
                return NotFound(new { message = "Department not found or name already exists" });

            return Ok(new { data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _departmentService.DeleteDepartmentAsync(id);
            if (!deleted)
                return NotFound(new { message = "Department not found" });

            return Ok(new { message = "Department deleted successfully" });
        }
    }
}