using EmployeeManagementSystem.Entity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Data
{
    public class AppDBContext:DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options):base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Attendance> attendances { get; set; }

    }
}
