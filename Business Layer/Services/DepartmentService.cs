using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Department;
using Data_Layer.Data.Interfaces;
using Data_Layer.Data.Models;
using Data_Layer.Data.Repository;

namespace Business_Layer.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepo repo , IMapper mapper)
        {
            _departmentRepository = repo;
            _mapper = mapper;

        }

        public async Task<DepartmentListViewModel> CreateDepartmentAsync(DepartmentViewModel model)
        {
            if (!Regex.IsMatch(model.Code, @"^[A-Z]{4}$"))
                throw new ValidationException("Department code must be 4 uppercase letters");

            // Check uniqueness
            if (!await _departmentRepository.IsCodeUniqueAsync(model.Code))
                throw new ValidationException("Department code must be unique");

            if (!await _departmentRepository.IsNameUniqueAsync(model.Name))
                throw new ValidationException("Department name must be unique");

            // Map to entity
            var department = _mapper.Map<Department>(model);

            // Add to repository
            await _departmentRepository.AddAsync(department);
            await _departmentRepository.SaveChangesAsync();

            // Return view model
            return _mapper.Map<DepartmentListViewModel>(department);
        }
        public async Task UpdateDepartmentAsync(DepartmentViewModel model)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(model.Id);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            if (!await _departmentRepository.IsCodeUniqueAsync(model.Code, model.Id))
                throw new ValidationException("Department code must be unique");

            if (!await _departmentRepository.IsNameUniqueAsync(model.Name, model.Id))
                throw new ValidationException("Department name must be unique");

            department.Name = model.Name;
            department.Code = model.Code;
            department.Location = model.Location;

            await _departmentRepository.UpdateAsync(department);
            await _departmentRepository.SaveChangesAsync();
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException("Department not found");

            // Check if department has employees
            if (await _departmentRepository.GetEmployeeCountAsync(department.Code) > 0)
                throw new InvalidOperationException("Cannot delete department with employees");

            await _departmentRepository.DeleteAsync(department);
            await _departmentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DepartmentListViewModel>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentListViewModel>>(departments);
        }

        public async Task<DepartmentViewModel> GetDepartmentByCodeAsync(string code)
        {
            var department = await _departmentRepository.GetDepartmentByCodeAsync(code);
            return _mapper.Map<DepartmentViewModel>(department);
        }

        public async Task<DepartmentViewModel> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetDepartmentByIdAsync(id);
            return _mapper.Map<DepartmentViewModel>(department);
        }

        public async Task<int> GetEmployeeCountAsync(string departmentCode)
        {
            return await _departmentRepository.GetEmployeeCountAsync(departmentCode);
        }

        public async Task<bool> IsCodeUniqueAsync(string code, int? excludeId = null)
        {
            return await _departmentRepository.IsCodeUniqueAsync(code, excludeId);
        }

        public async Task<bool> IsNameUniqueAsync(string name, int? excludeId = null)
        {
            return await  _departmentRepository.IsNameUniqueAsync(name, excludeId);
        }

        
    }
}
