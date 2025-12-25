using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private IRepository<Department> irdep;
        public DepartmentController(IRepository<Department> deprep)
        {
            this.irdep = deprep;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddDepartment([FromBody] Department model)
        {
            await irdep.AddAsync(model);
            await irdep.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateDepartment([FromRoute] int id, [FromBody] Department model)
        {
            var oldDept = await irdep.FindByIdAsync(id);
            oldDept.Name = model.Name;
            irdep.Update(oldDept);
            await irdep.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllDepartments()
        {
            return Ok(await irdep.GetAll());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            var e = await irdep.FindByIdAsync(id);
            if (e == null)
            {
                return Ok("Department Not Found");
            }

            await irdep.DeleteAsync(id);
            await irdep.SaveChangesAsync();
            return Ok();

        }
    }
}
