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
    public class LeaveController : ControllerBase
    {
        private readonly IRepository<LeaveRequest> lrepo;
        private readonly IRepository<Employee> erepo;

        public LeaveController(IRepository<LeaveRequest> lrepo, IRepository<Employee> erepo)
        {
            this.lrepo = lrepo;
            this.erepo = erepo;
        }

        [HttpPost("apply")]
        [Authorize]
        public async Task<IActionResult> ApplyLeave([FromBody] LeaveRequest model)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);

            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();
            if (emp == null)
            {
                return NotFound("Employee Not Found!");
            }

            var leave = new LeaveRequest()
            {
                EmpId = emp.Id,
                LeaveType = model.LeaveType,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Reason = model.Reason,
                Status = "Pending",
            };
            await lrepo.AddAsync(leave);
            await lrepo.SaveChangesAsync();
            return Ok(new { message = "Leave applied successfully" });

        }


        [HttpPost("update-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] LeaveRequest model)
        {
            var leave = await lrepo.FindByIdAsync(model.Id.Value);
            if (leave == null)
            {
                return NotFound("No request found");
            }

            leave.Status = model.Status;
            await lrepo.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> CancelLeave(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);

            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();
            if (emp == null)
            {
                return NotFound("Employee not found");
            }

            var leave = await lrepo.FindByIdAsync(id);
            if (leave == null)
            {
                return NotFound("No leave found");
            }

            if (leave.EmpId != emp.Id)
            {
                return Unauthorized("You are not authorized for this request");
            }

            if (leave.Status != "Pending")
            {
                return BadRequest("Cannot cancel already processed");
            }

            leave.Status = "Cancelled";
            await lrepo.SaveChangesAsync();
            return Ok(new { message = "Leave cancelled successfully" });
        }

        [HttpGet("myleavelist")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeLeaveList()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var emp = (await erepo.GetAll(x => x.Email == email)).FirstOrDefault();
            if (emp == null)
            {
                return NotFound(new { message = "Employee Not Found" });
            }

            var leaves = await lrepo.GetAll(x => x.EmpId == emp.Id);
            return Ok(leaves);
        }

        [HttpGet("leaves-stack")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> leavesStackAdmin()
        {
            var Employees = await erepo.GetAll();
            var Leaves = await lrepo.GetAll();

            var result = from l in Leaves
                         join e in Employees
                         on l.EmpId equals e.Id
                         select new AdminLeaveDTO
                         {
                             LeaveId = l.Id.Value,
                             EmpId = e.Id,
                             EmpName = e.Name,
                             LeaveType = l.LeaveType,
                             StartDate = l.StartDate.Value,
                             EndDate = l.EndDate.Value,
                             Reason = l.Reason,
                             Status = l.Status
                         };
            return Ok(result);
        }
    }
}
