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

            // Helper method for setting alerts
            private void SetAlert(string type, string title, string message)
            {
                TempData["AlertType"] = type;
                TempData["AlertTitle"] = title;
                TempData["AlertMessage"] = message;
            }

            public async Task<IActionResult> Index()
            {
                var departments = await _departmentService.GetAllDepartmentsAsync();
                return View(departments);
            }

            public IActionResult Create()
            {
                return View(new DepartmentViewModel());
            }

            [HttpPost]
            public async Task<IActionResult> Create(DepartmentViewModel model)
            {
                if (!ModelState.IsValid)
                    return View(model);

                try
                {
                    await _departmentService.CreateDepartmentAsync(model);
                    SetAlert("success", "Success", "Department created successfully");
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    SetAlert("danger", "Validation Error", ex.Message);
                    return View(model);
                }
                catch (Exception ex)
                {
                    SetAlert("danger", "Error", $"Failed to create department: {ex.Message}");
                    return View(model);
                }
            }

            public async Task<IActionResult> Edit(int id)
            {
                try
                {
                    var department = await _departmentService.GetDepartmentByIdAsync(id);
                    if (department == null)
                    {
                        SetAlert("warning", "Not Found", "Department not found");
                        return RedirectToAction("Index");
                    }

                    return View(department);
                }
                catch (Exception ex)
                {
                    SetAlert("danger", "Error", $"Couldn't load department: {ex.Message}");
                    return RedirectToAction("Index");
                }
            }

            [HttpPost]
            public async Task<IActionResult> Edit(int id, DepartmentViewModel model)
            {
                if (id != model.Id)
                {
                    SetAlert("warning", "Invalid", "ID mismatch");
                    return RedirectToAction("Index");
                }

                if (!ModelState.IsValid)
                    return View(model);

                try
                {
                    await _departmentService.UpdateDepartmentAsync(model);
                    SetAlert("success", "Updated", "Department updated successfully");
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    SetAlert("danger", "Validation Error", ex.Message);
                    return View(model);
                }
                catch (Exception ex)
                {
                    SetAlert("danger", "Error", $"Failed to update department: {ex.Message}");
                    return View(model);
                }
            }

            [HttpPost]
            public async Task<IActionResult> Delete(int id)
            {
                try
                {
                    await _departmentService.DeleteDepartmentAsync(id);
                    SetAlert("success", "Deleted", "Department deleted successfully");
                    return RedirectToAction("Index");
                }
                catch (ValidationException ex)
                {
                    SetAlert("danger", "Validation Error", ex.Message);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    SetAlert("danger", "Error", $"Failed to delete department: {ex.Message}");
                    return RedirectToAction("Index");
                }
            }

            // AJAX endpoint for getting all departments
            [HttpGet]
            public async Task<IActionResult> GetAllDepartments()
            {
                try
                {
                    var departments = await _departmentService.GetAllDepartmentsAsync();
                    var result = departments.Select(d => new { code = d.Code, name = d.Name });
                    return Json(result);
                }
                catch (Exception ex)
                {
                    return Json(new List<object>());
                }
            }
        }
    }
