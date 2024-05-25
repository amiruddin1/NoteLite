using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Models.DTO;
using System.Security.Claims;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using NuGet.Protocol.Core.Types;

namespace NoteLite.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        private readonly NoteInterface _noteInterface;
        private readonly CategoryInterface _categoryInterface;
        private readonly UserInterface _userInterface;
        private readonly UserManager<User> _userManager;

        public UserController(NoteInterface noteInterface, CategoryInterface categoryInterface,
            UserManager<User> userManager, UserInterface userInterface)
        {
            _noteInterface = noteInterface;
            _categoryInterface = categoryInterface;
            _userManager = userManager;
            _userInterface = userInterface;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 2)
        {
            try
            {
                TempData["LoginResponse"] = TempData["login_response"];
                TempData["Addition_response"] = TempData["response_Addition"];
                TempData["Updation_response"] = TempData["response_updation"];
                TempData["Deletion_response"] = TempData["response_deletion"];

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;

                List<Note> tags = new List<Note>();
                var response = await _noteInterface.GetTaggedPost(id);
                foreach(var item in response)
                {
                    Note note = await _noteInterface.GetById(item.NoteId);
                    tags.Add(note);
                }
                ViewBag.TaggedNotes = tags;

                List<Tag> allData = await _noteInterface.GetAllTaggedData();
                ViewBag.AllTagData = allData;

                List<User> users = await _userInterface.GetAll();
                ViewBag.users = users;

                List<Category> categories = await _categoryInterface.GetAll();
                ViewBag.categorias = categories;
                Dictionary<int, int> categoryProductCount = new Dictionary<int, int>();
                foreach (var category in categories)
                {
                    var output = await _noteInterface.GetAllByCategoryUser(category.CategoryId,id);
                    int productCount = output.Count;
                    categoryProductCount.Add(category.CategoryId, productCount);
                }
                ViewBag.CategoryProductCount = categoryProductCount;

                List<Note> result = await _noteInterface.GetAllByUser(id);


                var paginatedData = result.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                int totalItems = result.Count;
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                ViewBag.PageNumber = pageNumber;
                ViewBag.TotalPages = totalPages;
                return View(paginatedData);
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

        public async Task<IActionResult> AddNew()
        {
            try
            {
                var categories = await _categoryInterface.GetAll();
                var categoryItems = new List<SelectListItem>();
                foreach (var category in categories)
                {
                    categoryItems.Add(new SelectListItem { Value = category.CategoryId.ToString(), Text = category.CategoryName });
                }
                ViewData["Categories"] = categoryItems;
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

        [HttpPost]
        public async Task<IActionResult> AddNew(NoteModel noteModel)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;
                if (ModelState.IsValid)
                {
                    noteModel.User_Id = id;
                    var result = await _noteInterface.AddNewNote(noteModel);
                    if (result == "Success")
                    {
                        TempData["response_Addition"] = "Done";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Addition_response = "Error";
                        return View();
                    }
                }
                else
                {
                    var categories = await _categoryInterface.GetAll();
                    var categoryItems = new List<SelectListItem>();
                    foreach (var category in categories)
                    {
                        categoryItems.Add(new SelectListItem { Value = category.CategoryId.ToString(), Text = category.CategoryName });
                    }
                    ViewData["Categories"] = categoryItems;
                    return View(noteModel);
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.GetUserAsync(User);
            var uid = user.Id;

            var categories = await _categoryInterface.GetAll();
            var users = await _userInterface.GetAll();

            var userList = new List<SelectListItem>();
            foreach(var userData in users)
            {
                if(userData.Id != uid) 
                {
                    userList.Add(new SelectListItem { Value = userData.Id.ToString(), Text = userData.FirstName + " " + userData.LastName });
                } 
            }

            var categoryItems = new List<SelectListItem>();
            foreach (var category in categories)
            {
                categoryItems.Add(new SelectListItem { Value = category.CategoryId.ToString(), Text = category.CategoryName });
            }
            ViewData["Categories"] = categoryItems;
            ViewData["Users"] = userList;
            Note noteData = await _noteInterface.GetById(id);
            return View(noteData);
        }
        [HttpPost]
        public async Task<IActionResult> noteDetailPage(Note note)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _noteInterface.EditNote(note);
                    if (result == "Success")
                    {
                        TempData["response_updation"] = "Done";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.response_updation = "Error!";
                        return View();
                    }
                }
                else
                {
                    return View(note);
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _noteInterface.DeleteTaggedPosts(id);
                if (response)
                {
                    var result = await _noteInterface.RemoveNewNote(id);
                    if (result == "Success")
                    {
                        TempData["response_deletion"] = "Done";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
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

        public async Task<IActionResult> CategoryWiseProduct(int id)
        {
            try
            {
                if (id == 0)
                {
                    return View("Index");
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var uid = user.Id;
                var category = await _categoryInterface.GetById(id);
                ViewBag.category = category.CategoryName;
                List<Note> result = await _noteInterface.GetAllByCategoryUser(id, uid);
                return View(result);
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

        public async Task<IActionResult> Profile()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;
                User data = await _userManager.FindByIdAsync(id);
                return View(data);
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
        public async Task<IActionResult> GeneratePDF()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;
                User data = await _userManager.FindByIdAsync(id);
                List<Note> noteData = await _noteInterface.GetAllByUser(id);


                var document = new PdfDocument();
                string HtmlContent = "<div style='width: 100%; text-align: center'>";
                HtmlContent += "<h1>NoteLite</h1><h4>Notes Taken By: "+ user.FirstName +"  " + user.LastName + "</h4><br /><br /><br />";
                int res = 1;
                foreach(var note in noteData)
                {
                    HtmlContent += "<h3>("+ res + ")</h3><h3>Note Title: " + note.NoteTitle + "</h3><h4>Note Description:" + note.Description + " </h4><h4>Note Added on: " + note.NoteAdded + " </h4><br /><br />";
                    res++;
                }

                PdfGenerator.AddPdfPages(document, HtmlContent, PageSize.A4);
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }
                string Filename = "MyNotes.pdf";
                var FileResult = File(response, "application/pdf", Filename);

                return FileResult;
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

        public async Task<IActionResult> GenerateSingleNote(int id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var Uid = user.Id;
                User data = await _userManager.FindByIdAsync(Uid);
                Note note = await _noteInterface.GetById(id);


                var document = new PdfDocument();
                string HtmlContent = "<div style='width: 100%; text-align: center'>";
                HtmlContent += "<h1>NoteLite</h1><h4>Notes Taken By: " + user.FirstName + "  " + user.LastName + "</h4><br /><br /><br />";
                int res = 1;
                
                HtmlContent += "<h3>(" + res + ")</h3><h3>Note Title: " + note.NoteTitle + "</h3><h4>Note Description:" + note.Description + " </h4><h4>Note Added on: " + note.NoteAdded + " </h4><br /><br />";
                res++;
                

                PdfGenerator.AddPdfPages(document, HtmlContent, PageSize.A4);
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }
                string Filename = "MyNotes.pdf";
                var FileResult = File(response, "application/pdf", Filename);

                return FileResult;
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
        public async Task<IActionResult> search(string search)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.GetUserAsync(User);
                var id = user.Id;
                List<Note> list = await _noteInterface.GetAllBySearch(search, id);
                List<Category> category = new List<Category>();
                foreach(var item in list)
                {
                    Category cat = await _categoryInterface.GetById(item.CategoryId);
                    category.Add(cat);
                }
                ViewBag.categorias = category;
                return View(list);
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
        public async Task<IActionResult> tagPerson(int NoteId, string userId)
        {
            Tag tag = new Tag
            {
                NoteId = NoteId,
                UserId = userId,
                TagTimeSpan = DateTime.Now
            };
            var result = await _noteInterface.TagPerson(tag);
            if(result)
                return RedirectToAction("Index");
            else
            {
                return View();
            }
        }

    }
}
