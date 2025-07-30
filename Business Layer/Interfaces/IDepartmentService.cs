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
        Task DeleteDepartmentAsync(int id);
        Task<IEnumerable<DepartmentListViewModel>> GetAllDepartmentsAsync();
        Task<DepartmentViewModel> GetDepartmentByCodeAsync(string code);
        Task<DepartmentViewModel> GetDepartmentByIdAsync(int id);
        Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null);
        Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
        Task<int> GetEmployeeCountAsync(string departmentCode);
    }
}
