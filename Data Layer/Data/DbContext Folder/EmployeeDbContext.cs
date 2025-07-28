using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Layer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Data.DbContext_Folder
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
        : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Attendance> Attendances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //In case of Converting In‑Memory Database to actual one 

            // Department configuration
            modelBuilder.Entity<Department>(eb =>
            {
                eb.HasKey(d => d.Code);
                eb.HasIndex(d => d.Name).IsUnique();
                eb.HasIndex(d => d.Code).IsUnique();
                eb.Property(d => d.Code).HasMaxLength(4);
            });

            // Employee configuration
            modelBuilder.Entity<Employee>(eb =>
            {
                eb.HasKey(e => e.EmployeeCode);
                eb.Property(e => e.EmployeeCode)
                  .ValueGeneratedOnAdd();
                eb.HasIndex(e => e.Email).IsUnique();
                eb.HasOne(e => e.Department)
                  .WithMany(d => d.Employees)
                  .HasForeignKey(e => e.DepartmentCode);
            });

            // Attendance configuration
            modelBuilder.Entity<Attendance>(eb =>
            {
                eb.HasIndex(a => new { a.EmployeeCode, a.Date }).IsUnique();
                eb.Property(a => a.Status)
                  .HasConversion<int>();
            });

            // Seed sample Departments
            modelBuilder.Entity<Department>().HasData(
                new Department { Code = "HRMG",  Name = "Human Resources",  Location = "Cairo" },
                new Department { Code = "TECH", Name = "Technology",  Location = "Alexandria" }
            );

            // Seed sample Employees
            modelBuilder.Entity<Employee>().HasData(
                new Employee { FullName = "Ali Mohamed Hassan Omar", Email = "ali@example.com", DepartmentCode = "HRMG" },
                new Employee { FullName = "Sara Ahmed Ali Youssef", Email = "sara@example.com", DepartmentCode = "TECH" }
            );

            
        }
    }
}
    

