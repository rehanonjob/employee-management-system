using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendenceController : ControllerBase
    {
        private readonly IRepository<Employee> erepo;
        private readonly IRepository<Attendance> arepo;

        public AttendenceController(IRepository<Employee> erepo, IRepository<Attendance> arepo)
        {
            this.erepo = erepo;
            this.arepo = arepo;
        }

        public IRepository<Employee> Erepo { get; }

        [HttpPost("scan-in")]
        [Authorize]
        public async Task<IActionResult> ScanIn()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();

            var today = DateTime.UtcNow.Date;

            var alreadyScanned = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).Any();

            if (alreadyScanned)
            {
                return BadRequest(new { message = "Already scanned in today" });
            }

            var aten = new Attendance
            {
                EmpId = emp.Id,
                Date = today,
                ScanInTime = DateTime.UtcNow,
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

            var today = DateTime.UtcNow.Date;

            var attendence = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).FirstOrDefault();

            if (attendence == null)
            {
                return BadRequest(new { message = "Scan-in not found" });
            }

            attendence.ScanOffTime = DateTime.UtcNow;

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

            var today = DateTime.UtcNow.Date;

            var attendence = (await arepo.GetAll(x => x.EmpId == emp.Id && x.Date == today)).FirstOrDefault();

            return Ok(new
            {
                scannedIn = attendence != null && attendence.ScanInTime != default,
                scannedOff = attendence?.ScanOffTime != null
            });

        }
    }
}
