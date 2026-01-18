using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Entity
{
    public class PaySlip
    {
        [Key]
        public int PayslipId { get; set; }
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public int PaidDays { get; set; }
        public int TotalWorkingDays { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public Decimal TotalEarnings { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public Decimal TotalDeductions { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public Decimal NetPayable { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}
