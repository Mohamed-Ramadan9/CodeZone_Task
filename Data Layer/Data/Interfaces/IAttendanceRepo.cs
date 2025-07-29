using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Interfaces
{
    public interface IAttendanceRepo : IRepository<Attendance>
    {
        Task<Attendance> GetAttendanceByIdAsync(int id);
        Task<Attendance> GetByEmployeeAndDateAsync(int employeeId, DateTime date);
        Task<IEnumerable<Attendance>> GetFilteredAttendanceAsync(
            string? departmentCode = null,
            int? employeeId = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
        Task<IEnumerable<Attendance>> GetMonthlyAttendanceAsync(
            int employeeId,
            int year,
            int month);

        Task<Dictionary<int, (int presents, int absents, double percentage)>>
            GetMonthlyStatsForEmployees(List<int> employeeCodes, int year, int month);
        Task<IEnumerable<Attendance>> GetByEmployeeAsync(int employeeCode);

    }
}
