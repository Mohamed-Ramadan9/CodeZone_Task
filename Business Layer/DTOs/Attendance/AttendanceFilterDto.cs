using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Attendance
{
    public class AttendanceFilterDto
    {
        public string DepartmentCode { get; set; }
        public int? EmployeeCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
