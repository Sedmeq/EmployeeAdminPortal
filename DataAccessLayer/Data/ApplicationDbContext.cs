using EmployeeAdminPortal.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Models.Models.Entities;

namespace DataAccessLayer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTimeLog> EmployeeTimeLogs { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; } // Yeni əlavə edilmiş

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure WorkSchedule entity
            modelBuilder.Entity<WorkSchedule>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.Property(e => e.StartTime)
                    .IsRequired();

                entity.Property(e => e.EndTime)
                    .IsRequired();

                entity.Property(e => e.RequiredWorkHours)
                    .IsRequired();

                entity.Property(e => e.MinimumWorkMinutes)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            // Configure Department entity
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            // Configure Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            // Configure Employee entity
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20);

                entity.Property(e => e.Salary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                // Configure relationships
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.Role)
                    .WithMany(r => r.Employees)
                    .HasForeignKey(e => e.RoleId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Yeni əlavə edilmiş WorkSchedule əlaqəsi
                entity.HasOne(e => e.WorkSchedule)
                    .WithMany(ws => ws.Employees)
                    .HasForeignKey(e => e.WorkScheduleId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Create indexes
                entity.HasIndex(e => e.Email)
                    .IsUnique();

                entity.HasIndex(e => e.DepartmentId);
                entity.HasIndex(e => e.RoleId);
                entity.HasIndex(e => e.WorkScheduleId); // Yeni index
            });

            // Configure EmployeeTimeLog entity
            modelBuilder.Entity<EmployeeTimeLog>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CheckInTime)
                    .IsRequired();

                entity.Property(e => e.CheckOutTime)
                    .IsRequired(false);

                entity.Property(e => e.WorkDuration)
                    .IsRequired(false);

                entity.Property(e => e.Notes)
                    .HasMaxLength(500);

                // Configure relationship
                entity.HasOne(e => e.Employee)
                    .WithMany(emp => emp.TimeLogs)
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Create indexes
                entity.HasIndex(e => new { e.EmployeeId, e.CheckInTime });
                entity.HasIndex(e => e.CheckInTime);
            });

            // Seed initial data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Work Schedules
            var workSchedules = new[]
            {
                new WorkSchedule
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999991"),
                    Name = "8-17",
                    Description = "Standard 8:00-17:00 work schedule",
                    StartTime = new TimeSpan(8, 0, 0),
                    EndTime = new TimeSpan(17, 0, 0),
                    RequiredWorkHours = 8,
                    MinimumWorkMinutes = 480,
                    MaxLatenessMinutes = 15,
                    MaxEarlyLeaveMinutes = 15,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new WorkSchedule
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999992"),
                    Name = "9-18",
                    Description = "Standard 9:00-18:00 work schedule",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    RequiredWorkHours = 8,
                    MinimumWorkMinutes = 480,
                    MaxLatenessMinutes = 15,
                    MaxEarlyLeaveMinutes = 15,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new WorkSchedule
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999993"),
                    Name = "9-14",
                    Description = "Morning shift 9:00-14:00",
                    StartTime = new TimeSpan(9, 0, 0),
                    EndTime = new TimeSpan(14, 0, 0),
                    RequiredWorkHours = 5,
                    MinimumWorkMinutes = 300,
                    MaxLatenessMinutes = 10,
                    MaxEarlyLeaveMinutes = 10,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                },
                new WorkSchedule
                {
                    Id = Guid.Parse("99999999-9999-9999-9999-999999999994"),
                    Name = "14-18",
                    Description = "Afternoon shift 14:00-18:00",
                    StartTime = new TimeSpan(14, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    RequiredWorkHours = 4,
                    MinimumWorkMinutes = 240,
                    MaxLatenessMinutes = 10,
                    MaxEarlyLeaveMinutes = 10,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                }
            };

            // Seed Departments
            var departments = new[]
            {
                new Department { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "IT", Description = "Information Technology Department", CreatedAt = DateTime.Now },
                new Department { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Marketing", Description = "Marketing Department", CreatedAt = DateTime.Now },
                new Department { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Finance", Description = "Finance Department", CreatedAt = DateTime.Now },
                new Department { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "HR", Description = "Human Resources Department", CreatedAt = DateTime.Now },
                new Department { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Sales", Description = "Sales Department", CreatedAt = DateTime.Now },
                new Department { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Operations", Description = "Operations Department", CreatedAt = DateTime.Now }
            };

            // Seed Roles
            var roles = new[]
            {
                new Role { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Boss", Description = "Company Boss - Full Access", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Boss-IT", Description = "IT Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Boss-Marketing", Description = "Marketing Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "Boss-Finance", Description = "Finance Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Boss-HR", Description = "HR Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), Name = "Boss-Sales", Description = "Sales Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Boss-Operations", Description = "Operations Department Boss", CreatedAt = DateTime.Now },
                new Role { Id = Guid.Parse("88888888-8888-8888-8888-888888888888"), Name = "Employee", Description = "Regular Employee", CreatedAt = DateTime.Now }
            };

            modelBuilder.Entity<WorkSchedule>().HasData(workSchedules);
            modelBuilder.Entity<Department>().HasData(departments);
            modelBuilder.Entity<Role>().HasData(roles);
        }
    }
}