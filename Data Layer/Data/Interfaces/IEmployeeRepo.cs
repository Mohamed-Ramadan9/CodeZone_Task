using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Interfaces
{
    public interface IEmployeeRepo : IRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetAllEmployeesWithDepartmentAsync();
        Task<Employee> GetEmployeeByCode(int code);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string departmentCode);


    }
}
