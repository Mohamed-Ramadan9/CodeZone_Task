using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Attendance;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Data_Layer.Data.Repository;

namespace Business_Layer.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepo _attendanceRepository;
        private readonly IEmployeeRepo _employeeRepository;
        private readonly IMapper _mapper;

        public AttendanceService(
            IAttendanceRepo attendanceRepository,
            IEmployeeRepo employeeRepository,
            IMapper mapper)
        {
            _attendanceRepository = attendanceRepository;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceViewModel> GetAttendanceByIdAsync(int id)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(id);
            return _mapper.Map<AttendanceViewModel>(attendance);
        }

        public async Task<AttendanceStatus> GetAttendanceStatusAsync(int employeeCode, DateTime date)
        {
            var attendance = await _attendanceRepository
                .GetByEmployeeAndDateAsync(employeeCode, date.Date);

            return attendance?.Status ?? AttendanceStatus.NotMarked;
        }

        public async Task<IEnumerable<AttendanceViewModel>> GetFilteredAttendanceAsync(string? departmentCode = null, int? employeeCode = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var records = await _attendanceRepository
                .GetFilteredAttendanceAsync(departmentCode, employeeCode, startDate, endDate);

            return _mapper.Map<IEnumerable<AttendanceViewModel>>(records);
        }

        public async Task<IEnumerable<AttendanceViewModel>> GetMonthlyAttendanceAsync(int employeeCode, int year, int month)
        {
            var records = await _attendanceRepository
                .GetMonthlyAttendanceAsync(employeeCode, year, month);

            return _mapper.Map<IEnumerable<AttendanceViewModel>>(records);
        }

        public async Task<Dictionary<int, (int presents, int absents, double percentage)>>
       GetMonthlyStatsForEmployees(List<int> employeeCodes, int year, int month)
        {
            return await _attendanceRepository.GetMonthlyStatsForEmployees(
                employeeCodes, year, month);
        }

        public async Task RecordAttendanceAsync(AttendanceRecordViewModel model)
        {
            // Validate date
            if (model.Date.Date > DateTime.Today)
                throw new ValidationException("Cannot mark future attendance");

            // Validate employee
            var employee = await _employeeRepository.GetEmployeeByCode(model.EmployeeCode);
            if (employee == null)
                throw new ValidationException("Invalid employee code");

            // Check existing attendance
            var existing = await _attendanceRepository
                .GetByEmployeeAndDateAsync(model.EmployeeCode, model.Date.Date);

            if (existing != null)
            {
                // Don't update, just throw an exception for duplicate
                throw new ValidationException($"Attendance for {employee.FullName} on {model.Date.Date:MM/dd/yyyy} already exists. You can only mark one attendance per employee per day.");
            }
            else
            {
                // Create new record
                var attendance = _mapper.Map<Attendance>(model);
                await _attendanceRepository.AddAsync(attendance);
            }

            await _attendanceRepository.SaveChangesAsync();
        }

        public async Task UpdateAttendanceAsync(AttendanceRecordViewModel model)
        {
            // Validate the date
            if (model.Date > DateTime.Today)
                throw new ValidationException("Cannot update future attendance");

            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(model.Id);
            if (attendance == null)
                throw new KeyNotFoundException("Attendance record not found");

            // Update only date and status (keep employee code unchanged)
            attendance.Date = model.Date;
            attendance.Status = model.Status;

            await _attendanceRepository.UpdateAsync(attendance);
            await _attendanceRepository.SaveChangesAsync();
        }

        public async Task DeleteAttendanceAsync(int attendanceId)
        {
            var attendance = await _attendanceRepository.GetAttendanceByIdAsync(attendanceId);
            if (attendance == null)
                throw new KeyNotFoundException("Attendance record not found");

            await _attendanceRepository.DeleteAsync(attendance);
            await _attendanceRepository.SaveChangesAsync();
        }

        public async Task<(int presents, int absents, double percentage)>
            GetMonthlyStatsForEmployee(int employeeCode, int year, int month)
        {
            var stats = await _attendanceRepository.GetMonthlyStatsForEmployees(
                new List<int> { employeeCode }, year, month);

            return stats.TryGetValue(employeeCode, out var result)
                ? result
                : (0, 0, 0);
        }

        public async Task DeleteEmployeeAttendanceAsync(int employeeCode)
        {
            var records = await _attendanceRepository.GetByEmployeeAsync(employeeCode);
            foreach (var record in records)
            {
                await _attendanceRepository.DeleteAsync(record);
            }
            await _attendanceRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<AttendanceViewModel>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            var attendances = await _attendanceRepository.GetByEmployeeAsync(employeeId);
            return attendances.Select(a => _mapper.Map<AttendanceViewModel>(a));
        }

        public async Task<PaginatedAttendanceListViewModel> GetPaginatedAttendanceAsync(int page = 1, int pageSize = 10)
        {
            var allRecords = await _attendanceRepository.GetAllAsync();
            var totalRecords = allRecords.Count();
            var totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            
            var pagedRecords = allRecords
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var attendanceViewModels = _mapper.Map<IEnumerable<AttendanceViewModel>>(pagedRecords);

            return new PaginatedAttendanceListViewModel
            {
                AttendanceRecords = attendanceViewModels,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                TotalRecords = totalRecords
            };
        }
    }
}
