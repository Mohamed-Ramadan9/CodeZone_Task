//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Data_Layer.Data.DbContext_Folder;

//namespace Data_Layer.Data.Custom_Validations
//{
//    public class UniqueEmailAttribute : ValidationAttribute
//    {
//        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//        {
//            var context = (EmployeeDbContext)validationContext
//                .GetService(typeof(EmployeeDbContext));

//            string email = value?.ToString();
//            if (context.Authors.Any(a => a.Email == email))
//                return new ValidationResult("Email already exists.");

//            return ValidationResult.Success;
//        }
//    }
//}
