using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.ViewModels.Employee;
using Data_Layer.Data.Models;

namespace Business_Layer.Interfaces
{
        public interface IEmployeeService
        {
            Task<EmployeeListViewModel> CreateEmployeeAsync(EmployeeListViewModel employee);
            Task UpdateEmployeeAsync(EmployeeListViewModel employee);
            Task DeleteEmployeeAsync(int id);
            Task<IEnumerable<EmployeeListViewModel>> GetAllEmployeesAsync();
            Task<EmployeeViewModel> GetEmployeeByCodeAsync(int code);
            Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentAsync(string departmentCode);
            Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null);
        Task<(int presents, int absents, double percentage)> GetCurrentMonthAttendanceAsync(int employeeCode);


        }
    }


