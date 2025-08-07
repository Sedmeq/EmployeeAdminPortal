using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;

namespace BusinessLogicLayer.Service
{
    public interface IEmployeeService
    {
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(Guid id);
        Employee AddEmployee(EmployeeDto employeeDto);
        Employee UpdateEmployee(Guid id, EmployeeDto updatedEmployee);
        bool DeleteEmployee(Guid id);
    }
}
