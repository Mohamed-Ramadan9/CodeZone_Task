using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Repository
{
    public class EmployeeRepository : Generic_Repository<Employee>, IEmployeeRepo
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Employee> GetEmployeeByCode(int code)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string departmentCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            throw new NotImplementedException();
        }
    }
}
