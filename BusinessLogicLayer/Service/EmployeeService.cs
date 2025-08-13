using DataAccessLayer.Data;
using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return _context.Employees.Find(id);
        }

        public async Task<Employee?> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            // Email mövcuddurmu yoxla
            var existingEmployee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == employeeDto.Email);

            if (existingEmployee is not null)
            {
                return null; // Email artıq mövcuddur
            }

            var employee = new Employee
            {
                Username = employeeDto.Username,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary,
                PasswordHash = ""
            };

            var hashedPassword = new PasswordHasher<Employee>()
                .HashPassword(employee, employeeDto.Password);

            employee.PasswordHash = hashedPassword;

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public Employee UpdateEmployee(Guid id, EmployeeDto updatedEmployee)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return null;
            }

            //employee.Name = updatedEmployee.Name;
            employee.Username = updatedEmployee.Username;
            employee.Email = updatedEmployee.Email;
            employee.Phone = updatedEmployee.Phone;
            employee.Salary = updatedEmployee.Salary;

            // Əgər yeni password verilmişsə, onu da update edirik
            if (!string.IsNullOrEmpty(updatedEmployee.Password))
            {
                var hashedPassword = new PasswordHasher<Employee>()
                    .HashPassword(employee, updatedEmployee.Password);
                employee.PasswordHash = hashedPassword;
            }

            _context.SaveChanges();
            return employee;
        }

        public bool DeleteEmployee(Guid id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return false;
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();
            return true;
        }
    }
}