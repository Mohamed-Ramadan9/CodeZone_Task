using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Data.Repository
{
    public class DepartmentRepository : Generic_Repository<Department>, IDepartmentRepo
    {
        private readonly EmployeeDbContext _context;

        public DepartmentRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Department> GetDepartmentByCodeAsync(string code)
        {
            return await _context.Departments
                .Include(d => d.Employees)      
                .FirstOrDefaultAsync(d => d.Code == code);
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<int> GetEmployeeCountAsync(string departmentCode)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .CountAsync(e => e.Department.Code == departmentCode);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            var query = _context.Departments.AsQueryable();

            if (excludeId.HasValue)
                // exclude the department with that Id (for edit scenarios)
                query = query.Where(d => d.Id != excludeId.Value);

            bool exists = await query
                .AnyAsync(d => d.Code.ToUpper() == code.ToUpper());

            // true = no duplicate found
            return !exists;
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            var query = _context.Departments.AsQueryable();

            if (excludeId.HasValue)
               
                query = query.Where(d => d.Id != excludeId.Value);

            bool exists = await query
                .AnyAsync(d => d.Name.ToLower() == name.ToLower());

            return !exists;
        }

        // Override GetAllAsync to include employees for proper employee count
        public new async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
