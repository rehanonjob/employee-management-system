
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmployeeManagementSystem.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbSet<T> dbs;
        private AppDBContext db;
        public Repository(AppDBContext dbc)
        {
            dbs = dbc.Set<T>();
            db = dbc;
        }
        public async Task AddAsync(T entity)
        {
            await dbs.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var e = await FindByIdAsync(id);
            if (e == null)
            {
                throw new Exception("Record not found");
            }
            dbs.Remove(e);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            var e = await dbs.FindAsync(id);
            return e;
        }

        public async Task<List<T>> GetAll()
        {
            var list = await dbs.ToListAsync();
            return list;
        }

        public async Task<List<T>> GetAll(Expression<Func<T , bool>> filter)
        {
            var list = await dbs.AsQueryable().Where(filter).ToListAsync();
            return list;
        }

        public async Task<int> SaveChangesAsync()
        {
            return (await db.SaveChangesAsync());
        }

        public void Update(T entity)
        {
            dbs.Update(entity);
        }
    }
}
