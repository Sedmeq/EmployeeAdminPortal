using DataAccessLayer.Data;
using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Models.Dto;

namespace BusinessLogicLayer.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeResponseDto>> GetAllEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    Username = e.Username,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null,
                    RoleId = e.RoleId,
                    RoleName = e.Role != null ? e.Role.Name : null,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(Guid departmentId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .Where(e => e.DepartmentId == departmentId)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    Username = e.Username,
                    Email = e.Email,
                    Phone = e.Phone,
                    Salary = e.Salary,
                    DepartmentId = e.DepartmentId,
                    DepartmentName = e.Department != null ? e.Department.Name : null,
                    RoleId = e.RoleId,
                    RoleName = e.Role != null ? e.Role.Name : null,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
                .ToListAsync();
        }

        public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return null;

            return new EmployeeResponseDto
            {
                Id = employee.Id,
                Username = employee.Username,
                Email = employee.Email,
                Phone = employee.Phone,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department?.Name,
                RoleId = employee.RoleId,
                RoleName = employee.Role?.Name,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = employee.UpdatedAt
            };
        }

        public async Task<EmployeeResponseDto?> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            // Email mövcuddurmu yoxla
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employeeDto.Email);

            if (existingEmployee != null)
                return null; // Email artıq mövcuddur

            var employee = new Employee
            {
                Username = employeeDto.Username,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary,
                DepartmentId = employeeDto.DepartmentId,
                RoleId = employeeDto.RoleId,
                PasswordHash = "",
                CreatedAt = DateTime.Now
            };

            var hashedPassword = new PasswordHasher<Employee>()
                .HashPassword(employee, employeeDto.Password);

            employee.PasswordHash = hashedPassword;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Get the employee with department and role info
            return await GetEmployeeByIdAsync(employee.Id);
        }

        public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, EmployeeDto updatedEmployeeDto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return null;

            // Check if email is being changed and if it already exists
            if (employee.Email != updatedEmployeeDto.Email)
            {
                var existingEmployee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == updatedEmployeeDto.Email && e.Id != id);
                if (existingEmployee != null)
                    return null;
            }

            employee.Username = updatedEmployeeDto.Username;
            employee.Email = updatedEmployeeDto.Email;
            employee.Phone = updatedEmployeeDto.Phone;
            employee.Salary = updatedEmployeeDto.Salary;
            employee.DepartmentId = updatedEmployeeDto.DepartmentId;
            employee.RoleId = updatedEmployeeDto.RoleId;
            employee.UpdatedAt = DateTime.Now;

            // Əgər yeni password verilmişsə, onu da update edirik
            if (!string.IsNullOrEmpty(updatedEmployeeDto.Password))
            {
                var hashedPassword = new PasswordHasher<Employee>()
                    .HashPassword(employee, updatedEmployeeDto.Password);
                employee.PasswordHash = hashedPassword;
            }

            await _context.SaveChangesAsync();
            return await GetEmployeeByIdAsync(employee.Id);
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Employee?> GetEmployeeEntityByIdAsync(Guid id)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        // Legacy methods for backward compatibility
        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.Include(e => e.Department).Include(e => e.Role).ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefault(e => e.Id == id);
        }

        public async Task<Employee?> AddEmployeesAsync(EmployeeDto employeeDto)
        {
            var result = await AddEmployeesAsync(employeeDto);
            if (result == null) return null;

            return await _context.Employees.FindAsync(result.Id);
        }

        public Employee UpdateEmployee(Guid id, EmployeeDto updatedEmployee)
        {
            var result = UpdateEmployeeAsync(id, updatedEmployee).Result;
            if (result == null) return null;

            return _context.Employees.Find(result.Id);
        }

        public bool DeleteEmployee(Guid id)
        {
            return DeleteEmployeeAsync(id).Result;
        }
    }
}