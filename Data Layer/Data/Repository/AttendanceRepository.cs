using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;

namespace Data_Layer.Data.Repository
{
    public class AttendanceRepository : Generic_Repository<Attendance>, IAttendanceRepo
    {
        private readonly EmployeeDbContext _context;

        public AttendanceRepository(EmployeeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<Attendance> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Attendance>> GetFilteredAttendanceAsync(string? departmentCode = null, int? employeeId = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Attendance>> GetMonthlyAttendanceAsync(int employeeId, int year, int month)
        {
            throw new NotImplementedException();
        }
    }
}
