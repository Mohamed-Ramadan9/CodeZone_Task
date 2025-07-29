using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer.ViewModels.Attendance;
using Data_Layer.Data.Models;

namespace Business_Layer.Interfaces
{
    public interface IAttendanceService
    {
        Task RecordAttendanceAsync(AttendanceRecordViewModel model);
        Task UpdateAttendanceAsync(int attendanceId, AttendanceStatus newStatus);
        Task DeleteAttendanceAsync(int attendanceId);
        Task<AttendanceViewModel> GetAttendanceByIdAsync(int id);
        Task<IEnumerable<AttendanceViewModel>> GetFilteredAttendanceAsync(
            string? departmentCode = null,
            int? employeeCode = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
        Task<AttendanceStatus> GetAttendanceStatusAsync(int employeeCode, DateTime date);
        Task<IEnumerable<AttendanceViewModel>> GetMonthlyAttendanceAsync(int employeeId, int year, int month);
        Task<Dictionary<int, (int presents, int absents, double percentage)>>
        GetMonthlyStatsForEmployees(List<int> employeeCodes, int year, int month);

        Task<(int presents, int absents, double percentage)>
          GetMonthlyStatsForEmployee(int employeeCode, int year, int month);
        Task DeleteEmployeeAttendanceAsync(int employeeCode);
    }
}
