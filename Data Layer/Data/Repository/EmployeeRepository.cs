using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Data.Repository
{
    public class EmployeeRepository : Generic_Repository<Employee>, IEmployeeRepo
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeByCode(int code)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EmployeeCode == code);  
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string departmentCode)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Where(e => e.Department.Code == departmentCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesWithDepartmentAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeCode = null)
        {
          
            var query = _context.Employees.AsQueryable();

            if (excludeCode.HasValue)
            {
                query = query.Where(e => e.EmployeeCode != excludeCode.Value);
            }

            bool exists = await query
                .AnyAsync(e => e.Email.ToLower() == email.ToLower());

            return !exists;
        }
    }
}
