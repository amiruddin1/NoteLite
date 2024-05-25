using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Repository;
using System.Collections.Generic;
using System.Security.Claims;

namespace NoteLite.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly NoteInterface _noteInterface;
        private readonly CategoryInterface _categoryInterface;
        private readonly UserInterface _userInterface;
        private readonly UserManager<User> _userManager;

        public AdminController(NoteInterface noteInterface, CategoryInterface categoryInterface, UserInterface userInterface, UserManager<User> userManager)
        {
            _noteInterface = noteInterface;
            _categoryInterface = categoryInterface;
            _userInterface = userInterface;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.LoginResponse = TempData["login_response"];
                ViewBag.Addition_response_Admin = TempData["Addition_response_Admin"];
                ViewBag.Deletion_response_Admin = TempData["Deletion_response_Admin"];
                ViewBag.Updation_response_Admin = TempData["Updation_response_Admin"];


                List<Category> categories = await _categoryInterface.GetAll();
                ViewBag.categorias = categories;
                List<Note> result = await _noteInterface.GetAll();
                return View(result);
            }
            catch(Exception ex)
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

        public async Task<IActionResult> ManageCategory(int id)
        {
            try
            {
                Category category = await _categoryInterface.GetById(id);
                List<Note> notes = await _noteInterface.GetAllByCategory(id);
                ViewBag.count = notes.Count;
                return View(category);
            }
            catch(Exception ex) 
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
        [HttpPost]
        public async Task<IActionResult> ManageCategory(Category category)
        {
            
            try
            {
                var result = await _categoryInterface.EditCategory(category);
                if (result == "Success")
                {
                    TempData["Updation_response_Admin"] = "Done";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.response = "Error While Updating Data.";
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

        public async Task<IActionResult> Removecategory(int id)
        {
            try
            {
                var result = await _categoryInterface.Removecategory(id);
                if (result == "Success")
                {
                    TempData["Deletion_response_Admin"] = "Done";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.response = "Error While Removing Note Data.";
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

        public async Task<IActionResult> noteDetailPage(int id)
        {
            Note result = await _noteInterface.GetById(id);
            User user = await _userInterface.GetByID(result.UserId);
            ViewBag.user = user;
            return View(result);
        }

        public async Task<IActionResult> RemoveNote(int id)
        {
            var response = await _noteInterface.DeleteTaggedPosts(id);
            if(response)
            {
                var result = await _noteInterface.RemoveNewNote(id);
                if (result == "Success")
                {
                    ViewBag.response = "Note Removed Successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.response = "Error While Removing Note Data.";
                    return View();
                }
            }
            ViewBag.response = "Error While Removing Note Data.";
            return View();

        }
        public async Task<IActionResult> AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            try
            {
                var result = await _categoryInterface.AddNewCategory(category);
                if (result == "Success")
                {
                    TempData["Addition_response_Admin"] = "Done";
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.response = "Error While Adding Note Data.";
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
        public async Task<IActionResult> ManageUsers()
        {
            try
            {
                ViewBag.UDelete_response_Admin = TempData["UDelete_response_Admin"];
                List<User> users = await _userInterface.GetAll();
                List<User> newUsers = new List<User>();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;

                foreach (var item in users)
                {
                    if (item.Id != id)
                    {
                        newUsers.Add(item);
                    }
                }
                return View(newUsers);
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _userInterface.RemoveUserByID(id);
                if (result == "Success")
                {
                    TempData["UDelete_response_Admin"] = "Done";
                    return RedirectToAction("ManageUsers");
                }
                return View();
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
