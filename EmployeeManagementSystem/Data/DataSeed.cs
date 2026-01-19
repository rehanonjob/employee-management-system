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

    }
}
