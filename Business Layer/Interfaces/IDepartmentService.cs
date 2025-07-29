using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.ViewModels.Department;
using Data_Layer.Data.Models;

namespace Business_Layer.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentListViewModel> CreateDepartmentAsync(DepartmentViewModel department);
        Task UpdateDepartmentAsync(DepartmentViewModel department);
        Task DeleteDepartmentAsync(string code);
        Task<IEnumerable<DepartmentListViewModel>> GetAllDepartmentsAsync();
        Task<DepartmentViewModel> GetDepartmentByCodeAsync(string code);
        Task<bool> IsCodeUniqueAsync(string code, string? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, string? excludeId = null);
        Task<int> GetEmployeeCountAsync(string departmentCode);
    }
}
