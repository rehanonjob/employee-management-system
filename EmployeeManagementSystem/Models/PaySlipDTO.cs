using EmployeeManagementSystem.Entity;

namespace EmployeeManagementSystem.Models
{
    public class PaySlipDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime JoiningDate { get; set; }


        public int DeptId { get; set; }
        public string DeptName { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AttenDays { get; set; }
        public int DayInMonth { get; set; }
        public int PaidDays { get; set; }


        public Decimal? BasicSalary { get; set; }
        public Decimal TotalEarned { get; set; }
        public Decimal HRA { get; set; }
        public Decimal Conveyance { get; set; }
        public Decimal Educational { get; set; }
        public Decimal DriverAllowance { get; set; }
        public Decimal TotalEarnedAmount { get; set; }


        public Decimal PTAX { get; set; }
        public Decimal PFT { get; set; }
        public Decimal TotalDeductions { get; set; }
        public Decimal BasicTotalAmount { get; set; }

        public Decimal NetPayable { get; set; }
    }
}
