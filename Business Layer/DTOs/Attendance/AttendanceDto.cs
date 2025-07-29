using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.Models;

namespace Business_Layer.DTOs.Attendance
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public AttendanceStatus Status { get; set; }
        public int EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentCode { get; set; }
    }
}
