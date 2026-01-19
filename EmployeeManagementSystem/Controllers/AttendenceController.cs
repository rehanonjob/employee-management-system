using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IRepository<Employee> erepo;
        private readonly IRepository<Attendance> arepo;
        private readonly IRepository<Department> drepo;
        private readonly IRepository<PaySlip> prepo;

        public AttendenceController(IRepository<Employee> erepo, IRepository<Attendance> arepo, IRepository<Department> drepo, IRepository<PaySlip> prepo)
        {
            this.erepo = erepo;
            this.arepo = arepo;
            this.drepo = drepo;
            this.prepo = prepo;
        }

        public IRepository<Employee> Erepo { get; }

        [HttpPost("scan-in")]
        [Authorize]
        public async Task<IActionResult> ScanIn()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();

            var today = DateTime.Now.Date;

            var alreadyScanned = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).Any();

            if (alreadyScanned)
            {
                return BadRequest(new { message = "Already scanned in today" });
            }

            var aten = new Attendance
            {
                EmpId = emp.Id,
                Date = today,
                ScanInTime = DateTime.Now,
                Status = "Present"
            };

            await arepo.AddAsync(aten);
            await arepo.SaveChangesAsync();
            return Ok(new { message = "Scan-in Successfull" });
        }


        [HttpPost("scan-off")]
        [Authorize]
        public async Task<IActionResult> ScanOff()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();

            var today = DateTime.Now.Date;

            var attendence = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).FirstOrDefault();

            if (attendence == null)
            {
                return BadRequest(new { message = "Scan-in not found" });
            }

            attendence.ScanOffTime = DateTime.Now;

            await arepo.SaveChangesAsync();
            return Ok(new { message = "Scan-off successful" });
        }


        [HttpGet("get-all-list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllList()
        {
            var attendances = await arepo.GetAll();
            var employees = await erepo.GetAll();

            var result =
                from a in attendances
                join e in employees
                on a.EmpId equals e.Id
                orderby a.CreatedAt descending
                select new AdminAttendanceDTO
                {
                    EmpId = e.Id,
                    EmpName = e.Name,
                    Date = a.Date,
                    ScanInTime = a.ScanInTime,
                    ScanOffTime = a.ScanOffTime,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                };

            return Ok(result);
        }

        [HttpGet("today-status")]
        [Authorize]
        public async Task<IActionResult> GetTodayStatus()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);

            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();

            if (emp == null)
            {
                return Unauthorized();
            }

            var today = DateTime.Now.Date;

            var attendence = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).FirstOrDefault();

            return Ok(new
            {
                scannedIn = attendence != null && attendence.ScanInTime != default,
                scannedOff = attendence?.ScanOffTime != null
            });

        }

        [HttpGet("GetPaySlip/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AttPaySlip(int id)
        {
            var employee = (await erepo.GetAll(x => x.Id == id)).FirstOrDefault();
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            DateTime today = DateTime.Today;
            int lastDay = DateTime.DaysInMonth(today.Year, today.Month);

            //comment below condition to generate payslip in middle of month
            if (today.Day != lastDay)
            {
                return BadRequest("Payslip can only be generated on the last day of the current month");
            }

            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;

            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1);

            var attendenceList = await arepo.GetAll(x =>
            x.EmpId == id &&
            x.Status == "Present" &&
            x.Date >= startDate &&
            x.Date < endDate
            );


            int DaysInMonth = DateTime.DaysInMonth(year, month);

            int sundayCount = 0;

            for (DateTime date = startDate; date < endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Sunday)
                {
                    sundayCount++;
                }
            }

            int AttendedDays = attendenceList.Count();

            decimal PerDayS = employee.BasicSalary.Value / DaysInMonth;
            int paidDays = AttendedDays + sundayCount;
            decimal totalEarned = PerDayS * paidDays;


            decimal hra = employee.BasicSalary.Value * 0.50m;
            decimal conveyance = employee.BasicSalary.Value * 0.20m;
            decimal educational = employee.BasicSalary.Value * 0.015m;
            decimal driverAllowance = employee.BasicSalary.Value * 0.07m;


            decimal ptax = 200m;
            decimal pft = employee.BasicSalary.Value * 0.125m;

            decimal basicTotalAmount = employee.BasicSalary.Value + hra + conveyance + educational + driverAllowance;
            decimal totalDeductions = ptax + pft;
            decimal totalEarnedAmount = totalEarned + hra + conveyance + educational + driverAllowance;
            decimal netPayable = totalEarnedAmount - totalDeductions;


            var empDept = (await drepo.GetAll(x => x.Id == employee.DepartmentId)).FirstOrDefault();

            var payslip = new PaySlipDTO
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.Name,
                Designation = employee.JobTitle,
                DateOfBirth = employee.DateOfBirth,
                JoiningDate = employee.JoiningDate,

                DeptId = empDept.Id,
                DeptName = empDept.Name,

                StartDate = startDate,
                EndDate = endDate,
                AttenDays = AttendedDays,
                DayInMonth = DaysInMonth,
                PaidDays = paidDays,

                BasicSalary = employee.BasicSalary,
                TotalEarned = totalEarned,
                HRA = hra,
                Conveyance = conveyance,
                Educational = educational,
                DriverAllowance = driverAllowance,
                TotalEarnedAmount = totalEarnedAmount,

                PTAX = ptax,
                PFT = pft,
                TotalDeductions = totalDeductions,
                BasicTotalAmount = basicTotalAmount,

                NetPayable = netPayable
            };
            return Ok(payslip);
        }

        [HttpPost("AddPAySlip")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddPaySlip([FromBody] AddPaySlipDTO psy)
        {

            bool alreadyGen = (await prepo.GetAll(x => x.EmployeeId == psy.EmployeeId && x.PayPeriodStart == psy.PayPeriodStart && x.PayPeriodEnd == psy.PayPeriodEnd)).Any();
            if (alreadyGen)
            {
                return BadRequest("PaySlip is already generated for this month");
            }

            var paydata = new PaySlip
            {
                EmployeeId = psy.EmployeeId,
                PayPeriodStart = psy.PayPeriodStart,
                PayPeriodEnd = psy.PayPeriodEnd,
                PaidDays = psy.PaidDays,
                TotalWorkingDays = psy.TotalWorkingDays,
                TotalEarnings = psy.TotalEarnings,
                TotalDeductions = psy.TotalDeductions,
                NetPayable = psy.NetPayable,
                CreatedDate = DateTime.Now,
                Status = "Generated"
            };

            await prepo.AddAsync(paydata);
            await prepo.SaveChangesAsync();

            return Ok();
        }

    }
}
