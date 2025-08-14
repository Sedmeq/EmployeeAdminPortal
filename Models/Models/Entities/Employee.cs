using Models.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace EmployeeAdminPortal.Models.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        public decimal Salary { get; set; }

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Foreign Keys
        public Guid? DepartmentId { get; set; }
        public Guid? RoleId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Department? Department { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<EmployeeTimeLog> TimeLogs { get; set; } = new List<EmployeeTimeLog>();
    }
}
