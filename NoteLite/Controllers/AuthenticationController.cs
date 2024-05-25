using Microsoft.AspNetCore.Mvc;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Models.DTO;

namespace NoteLite.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserInterface _userInterface;

        public AuthenticationController(UserInterface userInterface)
        {
            _userInterface = userInterface;
        }
        public IActionResult Login()
        {
            ViewBag.Logout_response = TempData["logout_response"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginModel);
                }
                else
                {
                    var result =await _userInterface.Login(loginModel);
                    if(result == "User Logged in Successfully")
                    {
                        TempData["login_response"] = "User login";
                        return RedirectToAction("Index", "User");
                    }
                    else if (result == "Admin Logged in Successfully")
                    {
                        TempData["login_response"] = "Admin login";
                        return RedirectToAction("Index", "Admin");
                    }
                    else if(result == "Enter Valid Credentials")
                    {
                        ViewBag.login_response = "Error";
                        return View();
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel()
                {
                    InnerException = ex.InnerException.ToString(),
                    ExceptionMessage = ex.Message
                };
                TempData["ErrorViewModel"] = error;
                return RedirectToAction("ExceptionError", "Error");
            }
        }
        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _userInterface.Logout();
                if (result)
                {
                    TempData["logout_response"] = "Logout Done";
                }
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel()
                {
                    InnerException = ex.InnerException.ToString(),
                    ExceptionMessage = ex.Message
                };
                TempData["ErrorViewModel"] = error;
                return RedirectToAction("ExceptionError", "Error");
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(registerModel);
                }
                else
                {
                    var result = await _userInterface.Register(registerModel);
                    ViewBag.response = result;
                    if (result== "User Added Successfully")
                    {
                        return View("Login");
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                ErrorViewModel error = new ErrorViewModel()
                {
                    InnerException = ex.InnerException.ToString(),
                    ExceptionMessage = ex.Message
                };
                TempData["ErrorViewModel"] = error;
                return RedirectToAction("ExceptionError", "Error");
            }
        }
    }
}
