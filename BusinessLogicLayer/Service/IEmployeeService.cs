using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;
using Models.Models.Dto;
namespace BusinessLogicLayer.Service
{
    public interface IEmployeeService
    {
        Task<List<EmployeeResponseDto>> GetAllEmployeesAsync();
        Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(Guid departmentId);
        Task<EmployeeResponseDto?> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeResponseDto?> AddEmployeeAsync(EmployeeDto employeeDto);
        Task<EmployeeResponseDto?> UpdateEmployeeAsync(Guid id, EmployeeDto updatedEmployee);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<Employee?> GetEmployeeEntityByIdAsync(Guid id);         
    }
}
