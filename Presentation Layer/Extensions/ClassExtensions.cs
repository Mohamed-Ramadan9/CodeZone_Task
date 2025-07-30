using Microsoft.AspNetCore.Mvc;

namespace Mohamed_Ramadan_Code_Zone_Task.Extensions
{
    public  static class ClassExtensions
    {
        public static IActionResult WithDanger(this IActionResult result, Controller controller, string title, string message)
        {
            controller.TempData["AlertType"] = "danger";
            controller.TempData["AlertTitle"] = title;
            controller.TempData["AlertMessage"] = message;
            return result;
        }
        public static IActionResult WithSuccess(this IActionResult result, Controller controller, string title, string message)
        {
            controller.TempData["AlertType"] = "success";
            controller.TempData["AlertTitle"] = title;
            controller.TempData["AlertMessage"] = message;
            return result;

        }

        public static IActionResult WithWarning(this IActionResult result, Controller controller, string title, string message)
        {
            controller.TempData["AlertType"] = "warning";
            controller.TempData["AlertTitle"] = title;
            controller.TempData["AlertMessage"] = message;
            return result;
        }
    }
}
