using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Employee;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Data_Layer.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Business_Layer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo _repo;
        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepo repo ,
            IAttendanceRepo attendanceRepo ,
            IAttendanceService attendanceService,
            IMapper mapper)
        { 
        _repo = repo;
        _attendanceService = attendanceService;
        _mapper = mapper;
        
        }
        public async Task CreateEmployeeAsync(EmployeeViewModel model)
        {
            if (!ValidateFullName(model.FullName))
                throw new ValidationException("Full name must consist of four names, each at least 2 characters");

            if (!await _repo.IsEmailUniqueAsync(model.Email))
                throw new ValidationException("Email must be unique");

            var employee = _mapper.Map<Employee>(model);

            // Add to repository
            await _repo.AddAsync(employee);
            await _repo.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(EmployeeViewModel employee)
        {
            var existing = await _repo.GetEmployeeByCode(employee.EmployeeCode);
            if (existing == null)
                throw new KeyNotFoundException("Employee not found");

            if (!ValidateFullName(employee.FullName))
                throw new ValidationException("Full name must consist of four names, each at least 2 characters");

            if (!await _repo.IsEmailUniqueAsync(employee.Email, employee.EmployeeCode))
                throw new ValidationException("Email must be unique");

            existing.FullName = employee.FullName;
            existing.Email = employee.Email;
            existing.DepartmentId = employee.DepartmentId;

            await _repo.UpdateAsync(existing);
            await _repo.SaveChangesAsync();
        }
        public async Task DeleteEmployeeAsync(int code)
        {
           var emp = await _repo.GetEmployeeByCode(code);
            if (emp == null)
                throw new KeyNotFoundException("Employee not found");


            // Use attendance service to delete records
            await _attendanceService.DeleteEmployeeAttendanceAsync(code);
            _repo.DetachEntity(emp);
            await _repo.DeleteAsync(emp);
            await _repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<EmployeeListViewModel>> GetAllEmployeesAsync()
        {
            var employees = await _repo.GetAllEmployeesWithDepartmentAsync();
            var employeeCodes = employees.Select(e => e.EmployeeCode).ToList();
            var now = DateTime.Now;
            // Get attendance stats in bulk
            var attendanceStats = await _attendanceService
                .GetMonthlyStatsForEmployees(employeeCodes, now.Year, now.Month);

            return employees.Select(employee =>
            {
                var vm = _mapper.Map<EmployeeListViewModel>(employee);

                if (attendanceStats.TryGetValue(employee.EmployeeCode, out var stats))
                {
                    vm.PresentCount = stats.presents;
                    vm.AbsentCount = stats.absents;
                    vm.AttendancePercentage = stats.percentage;
                }
                return vm;
            }).ToList();
        }

        public async Task<PaginatedEmployeeListViewModel> GetPaginatedEmployeesAsync(int page = 1, int pageSize = 10)
        {
            var allEmployees = await _repo.GetAllEmployeesWithDepartmentAsync();
            var totalCount = allEmployees.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            // Apply pagination
            var pagedEmployees = allEmployees
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var employeeCodes = pagedEmployees.Select(e => e.EmployeeCode).ToList();
            var now = DateTime.Now;
            
            // Get attendance stats in bulk for the current page
            var attendanceStats = await _attendanceService
                .GetMonthlyStatsForEmployees(employeeCodes, now.Year, now.Month);

            var employeeViewModels = pagedEmployees.Select(employee =>
            {
                var vm = _mapper.Map<EmployeeListViewModel>(employee);

                if (attendanceStats.TryGetValue(employee.EmployeeCode, out var stats))
                {
                    vm.PresentCount = stats.presents;
                    vm.AbsentCount = stats.absents;
                    vm.AttendancePercentage = stats.percentage;
                }
                return vm;
            }).ToList();

            return new PaginatedEmployeeListViewModel
            {
                Employees = employeeViewModels,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<EmployeeViewModel> GetEmployeeByCodeAsync(int code)
        {
           var emp = await _repo.GetEmployeeByCode(code);

            return _mapper.Map<EmployeeViewModel>(emp);
            
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetEmployeesByDepartmentAsync(string departmentCode)
        {
            var emp = await _repo.GetEmployeesByDepartmentAsync(departmentCode);
            var employeeCodes = emp.Select(e => e.EmployeeCode).ToList();
            var now = DateTime.Now;

            // Get attendance stats
             var attendanceStats = await _attendanceService
                .GetMonthlyStatsForEmployees(employeeCodes, now.Year, now.Month);

            return emp.Select(e =>
            {
                var vm = _mapper.Map<EmployeeListViewModel>(e);

                if (attendanceStats.TryGetValue(e.EmployeeCode, out var stats))
                {
                    vm.PresentCount = stats.presents;
                    vm.AbsentCount = stats.absents;
                    vm.AttendancePercentage = stats.percentage;
                }
                return vm;
            });
        }
         public async Task<(int presents, int absents, double percentage)> GetCurrentMonthAttendanceAsync(int employeeCode)
       
         {
            var now = DateTime.Now;
            return await _attendanceService
                .GetMonthlyStatsForEmployee(employeeCode, now.Year, now.Month);
        }

        // EmployeeService.cs
        public async Task<List<EmployeeDropdownViewModel>> GetAllEmployeesForDropdownAsync()
        {
            var employees = await _repo.GetAllAsync();

            // Project to view model
            return employees.Select(e => new EmployeeDropdownViewModel
            {
                EmployeeCode = e.EmployeeCode,
                DisplayText = $"{e.FullName} (EMP{e.EmployeeCode:0000})"
            }).ToList();
        }

        public Task<bool> IsEmailUniqueAsync(string email, int? excludeId = null)
        {
            
              return _repo.IsEmailUniqueAsync(email, excludeId);
        }
        private bool ValidateFullName(string fullName)
        {
            var names = fullName?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return names != null && names.Length == 4 && names.All(n => n.Length >= 2);
        }

    }
}
