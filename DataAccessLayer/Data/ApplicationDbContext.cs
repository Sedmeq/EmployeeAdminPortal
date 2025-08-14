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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                    .HasMaxLength(500)
                    .IsRequired(false);

                // Configure relationship
                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Create index for better query performance
                entity.HasIndex(e => new { e.EmployeeId, e.CheckInTime });
                entity.HasIndex(e => e.CheckInTime);
            });

            // Configure Employee entity (if not already configured)
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
                    .HasMaxLength(20)
                    .IsRequired(false);

                entity.Property(e => e.Salary)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.PasswordHash)
                    .IsRequired();

                // Create unique index for email
                entity.HasIndex(e => e.Email)
                    .IsUnique();
            });
        }
    }
}
