namespace EmployeeManagementSystem.Models
{
    public class AddPaySlipDTO
    {
        public int EmployeeId { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public int PaidDays { get; set; }
        public int TotalWorkingDays { get; set; }
        public Decimal TotalEarnings { get; set; }
        public Decimal TotalDeductions { get; set; }
        public Decimal NetPayable { get; set; }
    }
}
