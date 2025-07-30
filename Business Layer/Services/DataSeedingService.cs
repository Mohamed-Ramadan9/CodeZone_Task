using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services
{
    public class DataSeedingService
    {
        private readonly EmployeeDbContext _context;

        public DataSeedingService(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task SeedDataAsync()
        {
            // Only seed if database is empty
            if (await _context.Departments.AnyAsync())
                return;

            // Seed Departments
            var departments = new List<Department>
            {
                new Department { Code = "HRMG", Name = "Human Resources", Location = "Cairo" },
                new Department { Code = "TECH", Name = "Technology", Location = "Alexandria" },
                new Department { Code = "MKTG", Name = "Marketing", Location = "Giza" },
                new Department { Code = "FINC", Name = "Finance", Location = "Cairo" }
            };

            await _context.Departments.AddRangeAsync(departments);
            await _context.SaveChangesAsync();

            // Get the saved departments to access their IDs
            var savedDepartments = await _context.Departments.ToListAsync();
            var hrmgDept = savedDepartments.First(d => d.Code == "HRMG");
            var techDept = savedDepartments.First(d => d.Code == "TECH");
            var mktgDept = savedDepartments.First(d => d.Code == "MKTG");
            var fincDept = savedDepartments.First(d => d.Code == "FINC");

            // Seed Employees
            var employees = new List<Employee>
            {
                new Employee { FullName = "Ali Mohamed Hassan Omar", Email = "ali@example.com", DepartmentId = hrmgDept.Id },
                new Employee { FullName = "Sara Ahmed Shabaan Youssef", Email = "sara@example.com", DepartmentId = techDept.Id },
                new Employee { FullName = "Omar Khalil Ibrahim Hassan", Email = "omar@example.com", DepartmentId = techDept.Id },
                new Employee { FullName = "Fatima Zoweil Ahmed Ali", Email = "fatima@example.com", DepartmentId = mktgDept.Id },
                new Employee { FullName = "Ahmed Mahmoud Hassan Omar", Email = "ahmed@example.com", DepartmentId = fincDept.Id },
                new Employee { FullName = "Nour Mohamed Ali Hassan", Email = "nour@example.com", DepartmentId = hrmgDept.Id },
                new Employee { FullName = "Youssef Khalil Ahmed Omar", Email = "youssef@example.com", DepartmentId = techDept.Id },
                new Employee { FullName = "Aisha Ramadan Hassan Ali", Email = "aisha@example.com", DepartmentId = mktgDept.Id },
                new Employee { FullName = "Hassan Mohamed Ali Omar", Email = "hassan@example.com", DepartmentId = techDept.Id },
                new Employee { FullName = "Layla Ahmed Hassan Youssef", Email = "layla@example.com", DepartmentId = mktgDept.Id },
                new Employee { FullName = "Karim Hussien Omar Hassan", Email = "karim@example.com", DepartmentId = fincDept.Id },
                new Employee { FullName = "Mariam Khalil Ahmed Ali", Email = "mariam@example.com", DepartmentId = hrmgDept.Id }
            };

            await _context.Employees.AddRangeAsync(employees);
            await _context.SaveChangesAsync();

            // Seed Attendance Records (last 7 days)
            var attendanceRecords = new List<Attendance>();
            var today = DateTime.Today;
            var random = new Random();

            foreach (var employee in employees)
            {
                for (int i = 0; i < 7; i++)
                {
                    var date = today.AddDays(-i);
                    if (date <= today) // Don't create future records
                    {
                        var status = random.Next(3); // 0=NotMarked, 1=Present, 2=Absent
                        var attendanceStatus = (AttendanceStatus)status;
                        
                        attendanceRecords.Add(new Attendance
                        {
                            EmployeeCode = employee.EmployeeCode,
                            Date = date,
                            Status = attendanceStatus
                        });
                    }
                }
            }

            await _context.Attendances.AddRangeAsync(attendanceRecords);
            await _context.SaveChangesAsync();
        }
    }
} 