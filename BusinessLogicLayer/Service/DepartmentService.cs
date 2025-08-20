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
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    CreatedAt = d.CreatedAt,
                    EmployeeCount = d.Employees.Count
                })
                .ToListAsync();
        }

        public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return null;

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt,
                EmployeeCount = department.Employees.Count
            };
        }

        public async Task<DepartmentResponseDto?> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            // Check if department name already exists
            if (await _context.Departments.AnyAsync(d => d.Name.ToLower() == departmentDto.Name.ToLower()))
                return null;

            var department = new Department
            {
                Id = Guid.NewGuid(),
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreatedAt = DateTime.Now
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt,
                EmployeeCount = 0
            };
        }

        public async Task<DepartmentResponseDto?> UpdateDepartmentAsync(Guid id, DepartmentDto departmentDto)
        {
            var department = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return null;

            // Check if new name conflicts with existing department
            if (await _context.Departments.AnyAsync(d => d.Id != id && d.Name.ToLower() == departmentDto.Name.ToLower()))
                return null;

            department.Name = departmentDto.Name;
            department.Description = departmentDto.Description;

            await _context.SaveChangesAsync();

            return new DepartmentResponseDto
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                CreatedAt = department.CreatedAt,
                EmployeeCount = department.Employees.Count
            };
        }

        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> DepartmentExistsAsync(Guid id)
        //{
        //    return await _context.Departments.AnyAsync(d => d.Id == id);
        //}
    }
}
