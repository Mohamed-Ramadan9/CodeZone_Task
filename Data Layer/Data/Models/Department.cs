using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data_Layer.Data.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; } // Auto-generated primary key

        [Required, StringLength(4, MinimumLength = 4), RegularExpression("^[A-Z]{4}$")]
        public string Code { get; set; }

        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }


        [Required, StringLength(100)]
        public string Location { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();


    }
}
