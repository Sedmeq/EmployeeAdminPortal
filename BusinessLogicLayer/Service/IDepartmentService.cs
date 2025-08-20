using Models.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Service
{
    public interface IDepartmentService
    {
        Task<List<DepartmentResponseDto>> GetAllDepartmentsAsync();
        Task<DepartmentResponseDto?> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentResponseDto?> CreateDepartmentAsync(DepartmentDto departmentDto);
        Task<DepartmentResponseDto?> UpdateDepartmentAsync(Guid id, DepartmentDto departmentDto);
        Task<bool> DeleteDepartmentAsync(Guid id);
        //Task<bool> DepartmentExistsAsync(Guid id);
    }
}
