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
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(new { data = roles, count = roles.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound(new { message = "Role not found" });

            return Ok(new { data = role });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _roleService.CreateRoleAsync(roleDto);
            if (created == null)
                return BadRequest(new { message = "Role name already exists" });

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new { data = created });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] RoleDto roleDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _roleService.UpdateRoleAsync(id, roleDto);
            if (updated == null)
                return NotFound(new { message = "Role not found or name already exists" });

            return Ok(new { data = updated });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _roleService.DeleteRoleAsync(id);
            if (!deleted)
                return NotFound(new { message = "Role not found" });

            return Ok(new { message = "Role deleted successfully" });
        }
    }
}