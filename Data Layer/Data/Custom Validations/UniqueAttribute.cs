//using System;
//using System.ComponentModel.DataAnnotations;
//using Data_Layer.Data.DbContext_Folder;
//using Microsoft.EntityFrameworkCore;

//[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
//public class UniqueAttribute : ValidationAttribute
//{
//    private readonly string _propertyName;
//    public UniqueAttribute(string propertyName)
//    {
//        _propertyName = propertyName;
//        ErrorMessage = "{0} must be unique.";
//    }

//    protected override ValidationResult IsValid(object value, ValidationContext context)
//    {
//        if (value == null) return ValidationResult.Success;

//        // resolve your DbContext from DI
//        var db = (EmployeeDbContext)context.GetService(typeof(EmployeeDbContext));
//        var entityType = context.ObjectType;
//        var keyProp = entityType.GetProperty(context.MemberName);
//        var keyVal = entityType.GetProperty(keyProp.Name).GetValue(context.ObjectInstance);

//        // build a query: Db.Set<entityType>().Where(p => p.property == value && p.Key != keyVal)
//        var set = db.Set(entityType);
//        var exists = set
//            .AsQueryable()
//            .Cast<object>()
//            .Any(e =>
//            {
//                var prop = entityType.GetProperty(_propertyName).GetValue(e);
//                var id = keyProp.GetValue(e);
//                return Equals(prop, value) && !Equals(id, keyVal);
//            });

//        return exists
//            ? new ValidationResult(string.Format(ErrorMessage, context.DisplayName))
//            : ValidationResult.Success;
//    }
//}
