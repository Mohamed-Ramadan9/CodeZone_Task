using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Department;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Mohamed_Ramadan_Code_Zone_Task.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }



    }
}
