using EmployeeAdminPortal.Models.Dto;
using EmployeeAdminPortal.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(EmployeeDto request);
        Task<Employee?> RegisterAsync(EmployeeDto request);
    }
}
