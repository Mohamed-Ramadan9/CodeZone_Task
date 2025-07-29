using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.Models;

namespace Business_Layer.ViewModels.Attendance
{
    public class AttendanceRecordViewModel
    {
        [Required]
        public int EmployeeCode { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        public AttendanceStatus Status { get; set; }
    }
}
