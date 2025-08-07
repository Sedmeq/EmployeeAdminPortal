using DataAccessLayer.Data;
using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;

namespace BusinessLogicLayer.Service
{
    public  class EmployeeService : IEmployeeService
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
        public Employee AddEmployee(EmployeeDto employeeDto)
        {
            var employee = new Employee
            {
                Name = employeeDto.Name,
                Email = employeeDto.Email,
                Phone = employeeDto.Phone,
                Salary = employeeDto.Salary
            };

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }
        public Employee UpdateEmployee(Guid id, EmployeeDto updatedEmployee)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                return null;
            }
            employee.Name = updatedEmployee.Name;
            employee.Email = updatedEmployee.Email;
            employee.Phone = updatedEmployee.Phone;
            employee.Salary = updatedEmployee.Salary;
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
