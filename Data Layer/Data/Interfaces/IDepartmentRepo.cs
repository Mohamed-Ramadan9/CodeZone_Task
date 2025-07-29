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
        Task<bool> IsCodeUniqueAsync(string code, string? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, string? excludeId = null);
        Task<int> GetEmployeeCountAsync(string departmentCode);
    }
}
