using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
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

        public async Task<T?> GetByIdAsync(object id)
        {
            // Get the primary key property using reflection
            var entityType = typeof(T);
            var keyProperty = entityType.GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null);

            if (keyProperty == null)
            {
                throw new InvalidOperationException($"Entity {entityType.Name} does not have a primary key defined with [Key] attribute.");
            }

            // Create a parameter expression for the entity
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "entity");

            // Create a property access expression for the key property
            var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, keyProperty);

            // Create a constant expression for the id value
            var idValue = System.Linq.Expressions.Expression.Constant(id);

            // Create an equality expression
            var equality = System.Linq.Expressions.Expression.Equal(propertyAccess, idValue);

            // Create a lambda expression
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(equality, parameter);

            // Execute the query
            return await _dbSet.FirstOrDefaultAsync(lambda);
        }
        public async Task AddAsync(T entity)
        {
            _dbSet.Add(entity);
            
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            
        }


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
