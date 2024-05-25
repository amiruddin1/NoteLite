using Microsoft.AspNetCore.Mvc;
using NoteLite.Models;

namespace NoteLite.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet("/Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult NotFound404()
        {
            ViewBag.Title = "Error 404 - File not Found";
            return View("Index");
        }

        
        public IActionResult ExceptionError()
        {
            if (TempData.ContainsKey("ErrorViewModel"))
            {
                var error = TempData["ErrorViewModel"] as ErrorViewModel;
                return View(error);
            }
            return View(null);
        }
    }
}
