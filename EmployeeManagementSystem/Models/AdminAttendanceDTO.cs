namespace EmployeeManagementSystem.Models
{
    public class AdminAttendanceDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime Date { get; set; }
        public DateTime ScanInTime { get; set; }
        public DateTime? ScanOffTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
