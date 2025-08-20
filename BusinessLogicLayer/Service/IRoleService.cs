using Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public interface IRoleService
    {
        Task<List<RoleResponseDto>> GetAllRolesAsync();
        Task<RoleResponseDto?> GetRoleByIdAsync(Guid id);
        Task<RoleResponseDto?> CreateRoleAsync(RoleDto roleDto);
        Task<RoleResponseDto?> UpdateRoleAsync(Guid id, RoleDto roleDto);
        Task<bool> DeleteRoleAsync(Guid id);
        //Task<bool> RoleExistsAsync(Guid id);
        //Task<string?> GetRoleNameAsync(Guid roleId);
    }
}
