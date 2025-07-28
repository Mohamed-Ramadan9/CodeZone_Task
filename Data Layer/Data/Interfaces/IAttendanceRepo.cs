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
    }
}
