using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Service
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            
            return hasher.HashPassword(null, password);
        }

        public bool VerifyPssword(string hash, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(null, hash, password);
            return result == PasswordVerificationResult.Success ? true: false;
        }
    }
}
