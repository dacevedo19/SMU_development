using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMU.Models;
using SMU.ViewModels;

namespace SMU.Controllers
{
    public class RequestController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IRequestManager requestManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        public RequestController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, 
            IRequestManager requestManager, IWebHostEnvironment hostingEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.requestManager = requestManager;
            this.hostingEnvironment = hostingEnvironment;            
        }


        [HttpGet]
        public IActionResult RegisterRequest()
        {
            var model = new RegisterRequestViewModel();            
            ViewBag.Today = DateTime.Today;
            model.ListOfTypes = GetRequestTypes();
            return View(model);
        }

       
        [HttpPost]
        public IActionResult RegisterRequest(RegisterRequestViewModel model)
        {
            Request request = new Request();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            if(model != null)
            {
                if (model.BeginDate > model.EndDate)
                {
                    ModelState.AddModelError("", "La fecha de fin debe ser mayor a la fecha de inicio.");                    
                    model.ListOfTypes = GetRequestTypes();
                }
                else {
                    string uniqueFileName = null;
                    if (model.Attachment != null)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attachment.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    request.UserId = userId;
                    request.RequestDate = DateTime.Now;
                    request.BeginDate = model.BeginDate;
                    request.EndDate = model.EndDate;
                    request.AttachmentPath = uniqueFileName;
                    request.Status = Status.Procesada;
                    request.Type = (RequestType)model.SelectedRequestType;               

                    if (requestManager.Create(request))
                        {
                            return RedirectToAction("MyRequests");
                        } else
                        {
                            ModelState.AddModelError("", "Ocurrió un error. Intente nuevamente revisando los datos ingresados");
                        }
                }
            }

            return View(model);
        }
        
        
        [HttpGet]
        public IActionResult MyRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Request> userRequests = requestManager.GetRequestsByUserId(userId);
            return View(userRequests);
        }

        
        public IActionResult DeleteRequest(int id)
        {
            Request request = requestManager.Find(id);

            if (request == null)
            {
                ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                return View("NotFound");
            }
            else
            {
               
                if (requestManager.Delete(id))
                {
                    return RedirectToAction("MyRequests");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrrió un error al procesar la solicitud. Intente nuevamente");
                }
                return View("MyRequests");
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> ManageSubordinatesRequests()
        {
            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser loggedUser = await userManager.FindByIdAsync(loggedUserId);
            int loggedUserDocument = loggedUser.Document;
            List<ManageSubordinatesRequestsViewModel> model = new List<ManageSubordinatesRequestsViewModel>();

            List<Request> subordinatesRequests = GetSubordinateRequests(loggedUserDocument);           

            foreach(Request r in subordinatesRequests)
            {
                AppUser requestUser = await userManager.FindByIdAsync(r.UserId);
                ManageSubordinatesRequestsViewModel m = new ManageSubordinatesRequestsViewModel
                {
                    Id = r.Id,
                    UserRequesting = requestUser.Name + " " + requestUser.Lastname,
                    Type = r.Type,
                    BeginDate = r.BeginDate,
                    EndDate = r.EndDate,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                };
                model.Add(m);
            }

            return View(model);
        }

        
        [HttpGet]
        public async Task<IActionResult> ManageAllRequestsAsync()
        {
            List<ManageSubordinatesRequestsViewModel> model = new List<ManageSubordinatesRequestsViewModel>();
            List<Request> requests = requestManager.GetRequests();

            foreach (Request r in requests)
            {               
                AppUser loggedUser = await userManager.FindByIdAsync(r.UserId);
                ManageSubordinatesRequestsViewModel m = new ManageSubordinatesRequestsViewModel
                {
                    Id = r.Id,
                    UserRequesting = loggedUser.Name + " " +loggedUser.Lastname,
                    Type = r.Type,
                    BeginDate = r.BeginDate,
                    EndDate = r.EndDate,
                    RequestDate = r.RequestDate,
                    Status = r.Status
                };
                model.Add(m);
            }

            return View(model);
        }


        public IActionResult AcceptSubordinateRequest(int id)
        {
            requestManager.Accept(id);
            return RedirectToAction("ManageSubordinatesRequests");
        }

        public IActionResult RejectSubordinateRequest(int id)
        {
            requestManager.Reject(id);
            return RedirectToAction("ManageSubordinatesRequests");
        }

        public IActionResult AcceptRequest(int id)
        {
            requestManager.Accept(id);
            return RedirectToAction("ManageAllRequests");
        }

        public IActionResult RejectRequest(int id)
        {
            requestManager.Reject(id);
            return RedirectToAction("ManageAllRequests");
        }

        #region Extra methods


        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult IsDateInFuture([Bind(Prefix = "BeginDate")] string aDate)
        {
            try
            {
                var date = DateTime.Parse(aDate);
                if (date > DateTime.Today) { return Json(true); }
                else { return Json("Debe seleccionar una fecha futura"); }
            } catch
            {
                return Json("Datos inválidos");
            }
        }

        private SelectList GetRequestTypes()
        {
            //return Enum.GetValues(typeof(RequestType)).Cast<RequestType>();
            var types = from RequestType s in Enum.GetValues(typeof(RequestType))
                           select new { ID = (int)s, Name = s.ToString() };
            return new SelectList(types, "ID", "Name");
        }

        private List<SelectListItem> ToListSelectListItem<T>()
        {
            var t = typeof(T);

            if (!t.IsEnum) { throw new ApplicationException("Tipo debe ser enum"); }

            var members = t.GetFields(BindingFlags.Public | BindingFlags.Static);

            var result = new List<SelectListItem>();

            foreach (var member in members)
            {
                var attributeDescription = member.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute),
                    false);
                var description = member.Name;

                if (attributeDescription.Any())
                {
                    description = ((System.ComponentModel.DescriptionAttribute)attributeDescription[0]).Description;
                }

                var value = ((int)Enum.Parse(t, member.Name));
                result.Add(new SelectListItem()
                {
                    Text = description,
                    Value = value.ToString()
                });
            }
            return result;
        }

        private List<Request> GetSubordinateRequests(int document)
        {
            List<Request> subordinatesRequests = new List<Request>();
            var users = userManager.Users.ToList();           

            foreach (AppUser u in users)
            {
                if (u.Supervisor == document)
                {
                    List<Request> listAux = requestManager.GetRequestsByUserId(u.Id);
                    subordinatesRequests.AddRange(listAux);
                }
            }
            return subordinatesRequests;
        }


        #endregion

    }
}