using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business_Layer.ViewModels.Attendance;
using Business_Layer.ViewModels.Department;
using Business_Layer.ViewModels.Employee;
using Data_Layer.Data.Models;

namespace Business_Layer.Profiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            // ─── Employee ───────────────────────────────────────

            // Entity → Edit/Create ViewModel
            CreateMap<Employee, EmployeeViewModel>();

            // Entity → List ViewModel (inherits from EmployeeViewModel)
            CreateMap<Employee, EmployeeListViewModel>()
                .IncludeBase<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.DepartmentName,
                           opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.DepartmentCode,
                           opt => opt.MapFrom(src => src.Department.Code))
                .ForMember(dest => dest.PresentCount,
                           opt => opt.Ignore()) // populate in service
                .ForMember(dest => dest.AbsentCount,
                           opt => opt.Ignore())
                .ForMember(dest => dest.AttendancePercentage,
                           opt => opt.Ignore());

            // Create/Edit ViewModel → Entity
            CreateMap<EmployeeViewModel, Employee>()
                // EmployeeListViewModel also maps back, but ignore read-only
                .ForMember(e => e.Department, opt => opt.Ignore());

            // ─── Department ─────────────────────────────────────

            // Entity → Edit/Create ViewModel
            CreateMap<Department, DepartmentViewModel>();

            // Entity → List ViewModel
            CreateMap<Department, DepartmentListViewModel>()
                .IncludeBase<Department, DepartmentViewModel>()
                .ForMember(dest => dest.EmployeeCount,
                           opt => opt.MapFrom(src => src.Employees.Count));

            // Create/Edit ViewModel → Entity
            CreateMap<DepartmentViewModel, Department>()
                .ForMember(d => d.Id, opt => opt.Ignore()); // Id is auto-generated

            // ─── Attendance ─────────────────────────────────────

            // Entity → Display ViewModel
            CreateMap<Attendance, AttendanceViewModel>()
                .ForMember(dest => dest.EmployeeName,
                           opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.DepartmentCode,
                           opt => opt.MapFrom(src => src.Employee.Department.Code));

            // Record ViewModel → Entity (for create/edit)
            CreateMap<AttendanceRecordViewModel, Attendance>()
                // Id is DB‑generated
                .ForMember(a => a.Id, opt => opt.Ignore())
                // navigation
                .ForMember(a => a.Employee, opt => opt.Ignore());

        }
    }
}
