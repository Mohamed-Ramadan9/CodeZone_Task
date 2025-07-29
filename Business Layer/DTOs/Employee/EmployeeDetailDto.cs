using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer.DTOs.Employee
{
    public class EmployeeDetailDto : EmployeeDTO
    {
        public string DepartmentName { get; set; }
        public string DepartmentLocation { get; set; }
    }
}
