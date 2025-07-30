using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Data.Repository
{
    public class AttendanceRepository : Generic_Repository<Attendance>, IAttendanceRepo
    {
        private readonly EmployeeDbContext _context;

        public AttendanceRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int id)
        {
            return await _context.Attendances
            .Include(a => a.Employee)        
            .ThenInclude(e => e.Department)  
             .AsNoTracking()                  
            .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attendance> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            return await _context.Attendances
            .Include(a => a.Employee)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.EmployeeCode == employeeId && a.Date.Date == date.Date);
        }

       

        public async Task<IEnumerable<Attendance>> GetFilteredAttendanceAsync(string? departmentCode = null, int? employeeId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Attendances
             .Include(a => a.Employee)
             .ThenInclude(e => e.Department)
             .AsNoTracking()
             .AsQueryable();

            if (!string.IsNullOrEmpty(departmentCode))
                query = query.Where(a => a.Employee.Department.Code == departmentCode);

            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeCode == employeeId.Value);

            if (startDate.HasValue)
                query = query.Where(a => a.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(a => a.Date <= endDate.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Attendance>> GetMonthlyAttendanceAsync(int employeeId, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Attendances
                .Where(a =>
                    a.EmployeeCode == employeeId &&
                    a.Date >= startDate &&
                    a.Date <= endDate)
                .ToListAsync();
        }

        public async Task<Dictionary<int, (int presents, int absents, double percentage)>>
        GetMonthlyStatsForEmployees(List<int> employeeCodes, int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var totalDays = (endDate - startDate).Days + 1;

            var stats = await _context.Attendances
                .Where(a => employeeCodes.Contains(a.EmployeeCode)
                        && a.Date >= startDate
                        && a.Date <= endDate)
                .GroupBy(a => a.EmployeeCode)
                .Select(g => new
                {
                    EmployeeCode = g.Key,
                    Presents = g.Count(a => a.Status == AttendanceStatus.Present),
                    Absents = g.Count(a => a.Status == AttendanceStatus.Absent)
                })
                .ToListAsync();

            

            return stats.ToDictionary(
                s => s.EmployeeCode,
                s => (
                    s.Presents,
                    s.Absents,
                    percentage: totalDays > 0 ? (s.Presents * 100.0) / totalDays : 0
                ));
        }
        public async Task<IEnumerable<Attendance>> GetByEmployeeAsync(int employeeCode)
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .Where(a => a.EmployeeCode == employeeCode)
                .ToListAsync();
        }

        // Override GetAllAsync to include related entities
        public new async Task<IEnumerable<Attendance>> GetAllAsync()
        {
            return await _context.Attendances
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
