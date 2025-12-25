using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Service;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Data
{
    public class DataSeed
    {
        private AppDBContext db;
        public DataSeed(AppDBContext dbc)
        {
            this.db= dbc;
        }

        //public void InsertData()
        //{
            //if(!db.Employees.Any())
            //{
            //    db.Employees.Add(
            //        new Employee { Name = "Auto-Emp 1"});
            //    db.Employees.Add(
            //        new Employee { Name = "Auto-Emp 2" });
            //}
                

            //if (!db.Users.Any())
            //{
            //    var passwordHelper = new PasswordHelper();
            //    db.Users.Add( new User()
            //    {
            //        Email= "admin@test.com",
            //        Password= passwordHelper.HashPassword("12345"),
            //            Role="Admin"
            //    });

            //    db.Users.Add(new User()
            //    {
            //        Email = "emp@test.com",
            //        Password = passwordHelper.HashPassword("12345"),
            //        Role = "Employee"
            //    });

            //}

            

            //db.SaveChanges();

        //}
    }
}
