using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Layer.Data.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeCode { get; set; }
        public Employee Employee { get; set; }

        [Required]
        public AttendanceStatus Status { get; set; } = AttendanceStatus.NotMarked;
    }

    public enum AttendanceStatus
    {
        NotMarked = 0,
        Present = 1,
        Absent = 2
        
    }

}
