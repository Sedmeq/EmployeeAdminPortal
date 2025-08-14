using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;
using Models.Models.Dto;
namespace BusinessLogicLayer.Service
{
    public interface IEmployeeService
    {
        // New async methods
        Task<List<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(Guid departmentId);
        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeResponseDto?> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, EmployeeDto updatedEmployee);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<Employee?> GetEmployeeEntityByIdAsync(Guid id);

        // Legacy methods for backward compatibility
        List<Employee> GetAllEmployees();
        Employee GetEmployeeById(Guid id);
        Task<Employee?> AddEmployeesAsync(EmployeeDto employeeDto);
        Employee UpdateEmployee(Guid id, EmployeeDto updatedEmployee);
        bool DeleteEmployee(Guid id);
    }
}
