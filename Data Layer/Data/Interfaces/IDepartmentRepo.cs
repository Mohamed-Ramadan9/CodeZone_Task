using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Interfaces
{
    public interface IDepartmentRepo : IRepository<Department>
    {
        Task<Department> GetDepartmentByCodeAsync(string code);
        Task<Department> GetDepartmentByIdAsync(int id);
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<int> GetEmployeeCountAsync(string departmentCode);
    }
}
