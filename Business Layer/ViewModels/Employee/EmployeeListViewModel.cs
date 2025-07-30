using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.ViewModels.Employee
{
    public class EmployeeListViewModel :EmployeeViewModel
    {
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; } // For display purposes
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public double AttendancePercentage { get; set; }

        public string EmployeeCodeDisplay => $"EMP{EmployeeCode:0000}";
        public string DepartmentDisplay => $"{DepartmentCode} - {DepartmentName}";
        public string AttendanceSummary => $"{PresentCount} Present, {AbsentCount} Absent";
        public string AttendancePercentageDisplay => $"{AttendancePercentage:0.0}%";
    }
}
