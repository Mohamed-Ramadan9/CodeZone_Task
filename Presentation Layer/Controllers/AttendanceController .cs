using System.ComponentModel.DataAnnotations;
using Business_Layer.Interfaces;
using Business_Layer.ViewModels.Attendance;
using Business_Layer.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mohamed_Ramadan_Code_Zone_Task.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeService _employeeService;

        public AttendanceController(
            IAttendanceService attendanceService,
            IEmployeeService employeeService)
        {
            _attendanceService = attendanceService;
            _employeeService = employeeService;
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
            try
            {
                var attendanceRecords = await _attendanceService.GetFilteredAttendanceAsync();
                return View(attendanceRecords);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Failed to load attendance records: {ex.Message}");
                return View(new List<AttendanceViewModel>());
            }
        }

        // Unified Attendance Dashboard
        public async Task<IActionResult> Dashboard(int page = 1, int pageSize = 10)
        {
            try
            {
                var paginatedAttendance = await _attendanceService.GetPaginatedAttendanceAsync(page, pageSize);
                return View(paginatedAttendance);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Failed to load attendance dashboard: {ex.Message}");
                return View(new PaginatedAttendanceListViewModel());
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.Employees = await _employeeService.GetAllEmployeesForDropdownAsync();
                return View();
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Failed to load employee data: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceRecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // this is not a business logic but method that calls the service and get the Selected list
                // made it for usability
                await ReloadEmployeeDropdown();
                return View(model);
            }

            try
            {
                await _attendanceService.RecordAttendanceAsync(model);
                SetAlert("success", "Success", "Attendance record added successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Error adding attendance record: {ex.Message}");
                await ReloadEmployeeDropdown();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var editModel = await _attendanceService.GetAttendanceRecordForEditAsync(id);
                if (editModel == null)
                {
                    SetAlert("warning", "Not Found", "Attendance record not found");
                    return RedirectToAction(nameof(Index));
                }
                // this is not a business logic but method that calls the service and get the Selected list
                // made it for usability
                await ReloadEmployeeDropdown();
                return View(editModel);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Couldn't load attendance record: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendanceRecordViewModel model)
        {
            if (id != model.Id)
            {
                SetAlert("warning", "Invalid", "ID mismatch");
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                await ReloadEmployeeDropdown();
                return View(model);
            }

            try
            {
                await _attendanceService.UpdateAttendanceAsync(model);
                SetAlert("success", "Updated", "Attendance record updated");
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                // Handle business rule violations
                SetAlert("danger", "Validation Error", ex.Message);
                await ReloadEmployeeDropdown();
                return View(model);
            }
            catch (KeyNotFoundException ex)
            {
                SetAlert("warning", "Not Found", ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Update failed: {ex.Message}");
                await ReloadEmployeeDropdown();
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var record = await _attendanceService.GetAttendanceByIdAsync(id);
                if (record == null)
                {
                    SetAlert("warning", "Not Found", "Attendance record not found");
                    return RedirectToAction(nameof(Index));
                }
                return View(record);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Couldn't load attendance record: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _attendanceService.DeleteAttendanceAsync(id);
                
                // Check if this is an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Attendance record deleted successfully" });
                }
                
                SetAlert("success", "Deleted", "Attendance record deleted successfully");
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Attendance record not found" });
                }
                
                SetAlert("danger", "Error", "Attendance record not found");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = $"Couldn't delete attendance record: {ex.Message}" });
                }
                
                SetAlert("danger", "Error", $"Couldn't delete attendance record: {ex.Message}");
                return RedirectToAction(nameof(Delete), new { id });
            }
        }



        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var record = await _attendanceService.GetAttendanceByIdAsync(id);
                if (record == null)
                {
                    SetAlert("warning", "Not Found", "Attendance record not found");
                    return RedirectToAction(nameof(Index));
                }
                return View(record);
            }
            catch (Exception ex)
            {
                SetAlert("danger", "Error", $"Couldn't load attendance details: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // Dynamic Attendance Page
        public IActionResult DynamicAttendance()
        {
            return View();
        }

        // AJAX Methods for Dynamic Attendance
        [HttpGet]
        public async Task<IActionResult> GetAttendanceStatus(int employeeCode, DateTime date)
        {
            try
            {
                var status = await _attendanceService.GetAttendanceStatusAsync(employeeCode, date);
                return Json(new { status = status.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { status = "NotMarked", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceStatus(string employeeCode, string date)
        {
            try
            {
                var result = await _attendanceService.GetAttendanceStatusByInputsAsync(employeeCode, date);

                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { status = "NotMarked", error = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAttendance(int employeeCode, DateTime date, string status)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendanceAsync(employeeCode, date, status);

                return Json(result);
            }
            catch (ValidationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while marking attendance" });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAttendanceRecord(int id, DateTime date, int status)
        {
            try
            {
                var result = await _attendanceService.UpdateAttendanceRecordAsync(id, date, status);

                return Json(result);
            }
            catch (ValidationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "An error occurred while updating attendance record" });
            }
        }


        private async Task ReloadEmployeeDropdown()
        {
            try
            {
                
                var employees = await _employeeService.GetAllEmployeesForDropdownAsync();

                
                ViewBag.Employees = new SelectList(employees, "EmployeeCode", "DisplayText");
            }
            catch (Exception ex)
            {
                ViewBag.Employees = new SelectList(new List<EmployeeDropdownViewModel>());
                SetAlert("warning", "Warning", $"Failed to load employees: {ex.Message}");
            }
        }

        // AJAX method to get attendance history for an employee
        [HttpGet]
        public async Task<IActionResult> GetAttendanceHistory(int employeeId)
        {
            try
            {
                var attendanceRecords = await _attendanceService.GetAttendanceByEmployeeAsync(employeeId);
                return PartialView("_AttendanceHistory", attendanceRecords);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // AJAX method to get attendance table partial view for dashboard
        [HttpGet]
        public async Task<IActionResult> GetAttendanceTablePartial(int page = 1, int pageSize = 10)
        {
            try
            {
                var paginatedAttendance = await _attendanceService.GetPaginatedAttendanceAsync(page, pageSize);
                return PartialView("_AttendanceTablePartial", paginatedAttendance);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // AJAX method to get all attendance records for filtering
        [HttpGet]
        public async Task<IActionResult> GetAllAttendanceRecords()
        {
            try
            {
                var allAttendanceRecords = await _attendanceService.GetFilteredAttendanceAsync();
                return Json(allAttendanceRecords);
            }
            catch (Exception ex)
            {
                return Json(new List<AttendanceViewModel>());
            }
        }
    }
}
    

