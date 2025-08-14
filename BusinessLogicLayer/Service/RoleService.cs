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
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleResponseDto>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Include(r => r.Employees)
                .Select(r => new RoleResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    CreatedAt = r.CreatedAt,
                    EmployeeCount = r.Employees.Count
                })
                .ToListAsync();
        }

        public async Task<RoleResponseDto?> GetRoleByIdAsync(Guid id)
        {
            var role = await _context.Roles
                .Include(r => r.Employees)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                return null;

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                EmployeeCount = role.Employees.Count
            };
        }

        public async Task<RoleResponseDto?> CreateRoleAsync(RoleDto roleDto)
        {
            // Check if role name already exists
            if (await _context.Roles.AnyAsync(r => r.Name.ToLower() == roleDto.Name.ToLower()))
                return null;

            var role = new Role
            {
                Id = Guid.NewGuid(),
                Name = roleDto.Name,
                Description = roleDto.Description,
                CreatedAt = DateTime.Now
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                EmployeeCount = 0
            };
        }

        public async Task<RoleResponseDto?> UpdateRoleAsync(Guid id, RoleDto roleDto)
        {
            var role = await _context.Roles
                .Include(r => r.Employees)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
                return null;

            // Check if new name conflicts with existing role
            if (await _context.Roles.AnyAsync(r => r.Id != id && r.Name.ToLower() == roleDto.Name.ToLower()))
                return null;

            role.Name = roleDto.Name;
            role.Description = roleDto.Description;

            await _context.SaveChangesAsync();

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                CreatedAt = role.CreatedAt,
                EmployeeCount = role.Employees.Count
            };
        }

        public async Task<bool> DeleteRoleAsync(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
                return false;

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RoleExistsAsync(Guid id)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id);
        }

        public async Task<string?> GetRoleNameAsync(Guid roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            return role?.Name;
        }
    }
}