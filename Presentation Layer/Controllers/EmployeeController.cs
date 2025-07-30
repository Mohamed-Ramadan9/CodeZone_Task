using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Employee;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mohamed_Ramadan_Code_Zone_Task.Extensions;

namespace Mohamed_Ramadan_Code_Zone_Task.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        public EmployeeController(
            IEmployeeService employeeService,
            IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

       

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            try
            {
                var paginatedEmployees = await _employeeService.GetPaginatedEmployeesAsync(page, pageSize);
                return View(paginatedEmployees);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Failed to load employees: {ex.Message}");
                return View(new PaginatedEmployeeListViewModel());
            }
        }

        public async Task<IActionResult> Create()
        {
            await LoadDepartmentOptions();
              return View(new EmployeeViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentOptions();
                return View(model);
            }

            try
            {
                await _employeeService.CreateEmployeeAsync(model);
                SetAlert("success", "Success", "Employee created successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Error creating employee: {ex.Message}");
                await LoadDepartmentOptions();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(id);
                if (employee == null)
                {
                    SetAlert("warning", "Not Found", "Employee not found");
                    return RedirectToAction(nameof(Index));
                }
                await LoadDepartmentOptions();
                return View(employee);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Couldn't load employee: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeViewModel model)
        {
            if (id != model.EmployeeCode)
            {
                SetAlert("warning", "Invalid Request", "Employee ID mismatch");
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await LoadDepartmentOptions();
                return View(model);
            }
                

            try
            {
                await _employeeService.UpdateEmployeeAsync(model);
                SetAlert("success", "Updated", "Employee updated successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Error updating employee: {ex.Message}");
                return View(model);
            }
        }

       

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(id);
                SetAlert("success", "Deleted", "Employee deleted successfully");
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Couldn't delete employee: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }



        private async Task LoadDepartmentOptions()
        {
            try
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                ViewBag.Departments = new SelectList(departments, "Id", "Name");
            }
            catch (Exception)
            {
                
                ViewBag.Departments = new SelectList(new List<string>());
            }
        }

        // Helper method for setting alerts
        private void SetAlert(string type, string title, string message)
        {
            TempData["AlertType"] = type;
            TempData["AlertTitle"] = title;
            TempData["AlertMessage"] = message;
        }

        // AJAX method for getting employees for dropdown
        [HttpGet]
        public async Task<IActionResult> GetEmployeesForDropdown()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesForDropdownAsync();
                return Json(employees);
            }
            catch (Exception ex)
            {
                return Json(new List<EmployeeDropdownViewModel>());
            }
        }
    }
}