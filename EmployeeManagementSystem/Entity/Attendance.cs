using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Entity
{
    public class Attendance
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Employee))]   
        public int EmpId { get; set; }

        public Employee Employee { get; set; }

        public DateTime Date { get; set; }
        public DateTime ScanInTime { get; set; }
        public DateTime? ScanOffTime { get; set; }
        public decimal? WorkingHours { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
