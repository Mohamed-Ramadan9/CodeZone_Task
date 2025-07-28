using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Repository
{
    public class DepartmentRepository : Generic_Repository<Department>, IDepartmentRepo
    {
        private readonly EmployeeDbContext _context;

        public DepartmentRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Department> GetDepartmentByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetEmployeeCountAsync(string departmentCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            throw new NotImplementedException();
        }
    }
}
