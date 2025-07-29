using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Employee
{
    public class EmployeeDTO
    {
        public int EmployeeCode { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]{2,}(?:\s+[a-zA-Z]{2,}){3}$",
            ErrorMessage = "Full name must be exactly four words, each at least two letters.")]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [RegularExpression("^[A-Z]{4}$",
             ErrorMessage = "Department code must be exactly 4 uppercase letters.")]
        public string DepartmentCode { get; set; } = string.Empty;
    }
}
