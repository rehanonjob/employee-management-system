namespace EmployeeManagementSystem.Models
{
    public class AdminLeaveDTO
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }
}
