using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public int EmployeeCode { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]{2,}(?:\s+[a-zA-Z]{2,}){3}$",
            ErrorMessage = "Full name must be four names, each at least 2 characters")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
