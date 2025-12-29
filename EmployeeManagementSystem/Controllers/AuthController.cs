using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IRepository<User> userRepo; 
        private readonly IConfiguration configuration;
        private readonly IRepository<Employee> empRepo;

        public AuthController(IRepository<User> userRepo, IConfiguration configuration, IRepository<Employee> empRepo)
        {
            this.userRepo = userRepo;
            this.configuration = configuration;
            this.empRepo = empRepo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthDto model)
        {
            var user = (await userRepo.GetAll(x => x.Email == model.Email)).FirstOrDefault();
            var name = (await empRepo.GetAll(o => o.Email == model.Email)).FirstOrDefault();

            if (user == null)
            {
                return new BadRequestObjectResult(new { message = "User Not Found" });
            }

            var passwordHelper = new PasswordHelper();

            if (!passwordHelper.VerifyPssword(user.Password, model.Password))
            {
                return new BadRequestObjectResult(new { message = "Email or Password Incorrect" });
            }

            var token = GenerateToken(user.Email, user.Role);
            return Ok(
                new AuthTokenDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Token = token,
                    Role = user.Role,
                    Name = name?.Name
                });
        }

        private string GenerateToken(string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,email),
                new Claim(ClaimTypes.Role,role)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfileData()
        {
            var curemail = User.FindFirstValue(ClaimTypes.Name);

            var curuser = (await userRepo.GetAll(x => x.Email == curemail)).FirstOrDefault();

            if (curuser == null)
            {
                return NotFound("User Not Found");
            }

            var curemp = (await empRepo.GetAll(x => x.UserId == curuser.Id)).FirstOrDefault();


            return Ok(new ProfileDTO
            {
                Name = curemp.Name,
                Email = curuser.Email,
                Phone = curemp.Phone,
                ProfileImage = curuser.ProfileImage
            });
        }


        [HttpPost("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(ProfileDTO ProfileData)
        {
            var curremail = User.FindFirstValue(ClaimTypes.Name);

            var curruser = (await userRepo.GetAll(x => x.Email == curremail)).FirstOrDefault();

            if (curruser == null)
            {
                return NotFound("User not found");
            }

            var emp = (await empRepo.GetAll(v => v.UserId == curruser.Id)).FirstOrDefault();

            if (emp != null)
            {
                emp.Email = ProfileData.Email;
                emp.Name = ProfileData.Name;
                emp.Phone = ProfileData.Phone;

                empRepo.Update(emp);
                await empRepo.SaveChangesAsync();
            }

            curruser.Email = ProfileData.Email;
            curruser.ProfileImage = ProfileData.ProfileImage;
            if (!string.IsNullOrEmpty(ProfileData.Password))
            {
                var passhelp = new PasswordHelper();
                curruser.Password = passhelp.HashPassword(ProfileData.Password);
            }
            userRepo.Update(curruser);
            await userRepo.SaveChangesAsync();
            return Ok(new { message = "Profile Updated Successfully" });
        }

        //[HttpPost("Profile")]
        //[Authorize]
        //public async Task<IActionResult> UpdateProfile([FromBody] ProfileDTO model)
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Name);
        //    if (email == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var user = (await userRepo.GetAll(x => x.Email == email)).FirstOrDefault();
        //    if (user == null)
        //    {
        //        return NotFound("User Not Found");
        //    }
        //    var employee = (await empRepo.GetAll(x => x.UserId == user.Id)).FirstOrDefault();
        //    if (employee != null)
        //    {
        //        employee.Name = model.Name;
        //        employee.Email = model.Email;
        //        employee.Phone = model.Phone;
        //        empRepo.Update(employee);
        //        await empRepo.SaveChangesAsync();
        //    }
        //    user.Email = model.Email;
        //    user.ProfileImage = model.ProfileImage;
        //    var passhelper = new PasswordHelper();
        //    user.Password = passhelper.HashPassword(model.Password);
        //    userRepo.Update(user);
        //    await userRepo.SaveChangesAsync();
        //    return Ok(new { message = "Profile Updated Successfully" });
        //}

        //[HttpGet("profile")]
        //[Authorize]
        //public async Task<IActionResult> GetProfile()
        //{
        //    var email = User.FindFirstValue(ClaimTypes.Name);
        //    if (email == null)
        //        return Unauthorized();

        //    var user = (await userRepo.GetAll(x => x.Email == email)).FirstOrDefault();
        //    if (user == null)
        //        return NotFound("User Not Found");

        //    var emp = (await empRepo.GetAll(x => x.UserId == user.Id)).FirstOrDefault();

        //    return Ok(new ProfileDTO()
        //    {
        //        Name = emp?.Name,            // ? = safe
        //        Email = user.Email,
        //        Phone = emp?.Phone,
        //        ProfileImage = user.ProfileImage
        //    });
        //}

    }


}
