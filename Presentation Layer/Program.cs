using Business_Layer.Interfaces;
using Business_Layer.Profiles;
using Business_Layer.Services;
using CodeZoneTask_MVC_.Interfaces;
using Data_Layer.Data.DbContext_Folder;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Mohamed_Ramadan_Code_Zone_Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
             options.UseInMemoryDatabase("EmployeeDb"));
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(Business_Layer.Profiles.MappingConfig).Assembly));

            // Register Business  Services
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<DataSeedingService>();

            // Register Repositories
            
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepository>();
            builder.Services.AddScoped<IEmployeeRepo, EmployeeRepository>();
            builder.Services.AddScoped<IAttendanceRepo, AttendanceRepository>();

            var app = builder.Build();

            // Seed the database
            using (var scope = app.Services.CreateScope())
            {
                var seedingService = scope.ServiceProvider.GetRequiredService<DataSeedingService>();
                seedingService.SeedDataAsync().Wait();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
