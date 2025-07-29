using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.DbContext_Folder;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Data.Repository
{
    public class Generic_Repository<T> : IRepository<T> where T : class
    {
        protected readonly EmployeeDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Generic_Repository(EmployeeDbContext db) 
        { _context = db; _dbSet = db.Set<T>(); }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _dbSet.ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return;
            _dbSet.Remove(entity);
            
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
