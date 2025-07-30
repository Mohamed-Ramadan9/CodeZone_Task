using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.ViewModels.Department
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [RegularExpression("^[A-Z]{4}$", ErrorMessage = "Department code must be 4 uppercase letters")]
        public string Code { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; }
    }
}
