using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private IRepository<Employee> erepo;
        private readonly IRepository<User> urepo;

        public EmployeeController(IRepository<Employee> repo, IRepository<User> urepo)
        {
            this.erepo = repo;
            this.urepo = urepo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetEmployees([FromQuery] SearchOptions searchOpt)
        {
            List<Employee> filterData;
            if (string.IsNullOrEmpty(searchOpt.Search))
            {
                filterData = await erepo.GetAll();
            }
            else
            {
                filterData = await erepo.GetAll(x =>
               x.Name.Contains(searchOpt.Search) ||
               x.Phone.Contains(searchOpt.Search) ||
               x.Email.Contains(searchOpt.Search)
               );
            }
            if (searchOpt.PageIndex.HasValue)
            {
                filterData = filterData.Skip(searchOpt.PageIndex.Value * searchOpt.PageSize.Value).Take(searchOpt.PageSize.Value).ToList();
            }
            return Ok(filterData);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeById([FromRoute] int id)
        {
            return Ok(await erepo.FindByIdAsync(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee emp)
        {
            var userClass = new User()
            {
                Email = emp.Email,
                Password = (new PasswordHelper()).HashPassword("12345"),
                Role = "Employee"
            };
            await urepo.AddAsync(userClass);
            await urepo.SaveChangesAsync();

            emp.UserId = userClass.Id;
            await erepo.AddAsync(emp);
            await erepo.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmploee([FromRoute] int id, [FromBody] Employee emp)
        {
            var oldEmp = await erepo.FindByIdAsync(id);
            oldEmp.Name = emp.Name;
            oldEmp.Email = emp.Email;
            oldEmp.Phone = emp.Phone;
            oldEmp.JobTitle = emp.JobTitle;
            oldEmp.DepartmentId = emp.DepartmentId;
            oldEmp.LastWorkingDate = emp.LastWorkingDate;
            erepo.Update(oldEmp);
            await erepo.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            await erepo.DeleteAsync(id);
            await erepo.SaveChangesAsync();
            return Ok();
        }
    }
}
