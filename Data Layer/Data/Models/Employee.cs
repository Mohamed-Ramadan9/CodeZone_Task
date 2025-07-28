using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_Layer.Data.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeCode { get; private set; }

        [Required, RegularExpression(@"^[a-zA-Z]{2,}(?:\s+[a-zA-Z]{2,}){3}$")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [ForeignKey("Department")]
        public string DepartmentCode { get; set; }
        public Department Department { get; set; }
    }

}
