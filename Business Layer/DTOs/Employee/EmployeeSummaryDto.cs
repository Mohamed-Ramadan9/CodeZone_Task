using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Employee
{
    public class EmployeeSummaryDto
    {
        public int EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
