using Identity.Entities;
using Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(UserDto request);
        Task<User?> RegisterAsync(UserDto request);
    }
}
