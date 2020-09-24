using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMU.Models;
using SMU.ViewModels;
using X.PagedList;
using X.PagedList.Mvc;


namespace SMU.Controllers
{
    public class RequestController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IRequestManager requestManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        static string searchTemp = "";

        public RequestController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, 
            IRequestManager requestManager, IWebHostEnvironment hostingEnvironment)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.requestManager = requestManager;
            this.hostingEnvironment = hostingEnvironment;            
        }


        #region Metodos

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


            if (model != null)
            {
                if (model.BeginDate > model.EndDate)
                {
                    ModelState.AddModelError("", "La fecha de fin debe ser mayor a la fecha de inicio.");
                    model.ListOfTypes = GetRequestTypes();
                }
                else
                {
                    string uniqueFileName;
                    string uniqueFileName2;
                    /*
                    string attch1 = null;
                    string attch2 = null;
                    if (model.Attachment != null) { attch1 = model.Attachment.ToString(); }
                    if (model.Attachment2 != null) { attch2 = model.Attachment.ToString(); }
                    RequestType type = (RequestType)model.SelectedRequestType;

                    if(type == RequestType.Vacacional && (attch1 != null || attch2 != null))
                    {
                        ModelState.AddModelError("", "No debe adjuntar un comprobante.");
                        model.SelectedRequestType = ;
                        return View(model);
                    }

                    if (attch1 == null && attch2 == null)
                    {
                        if (type == RequestType.Estudio || type == RequestType.Médica)
                        {
                            ModelState.AddModelError("", "Debe adjuntar un comprobante.");                            
                            return View(model);
                        }
                    } else if (attch1 == null || attch2 == null)
                    {
                        if(type == RequestType.Estudio)
                        {
                            ModelState.AddModelError("", "Debe adjuntar el comprobante y su respectivo posterior.");
                            return View(model);
                        } else if (type == RequestType.Médica && attch1 == null)
                        {
                            ModelState.AddModelError("", "Debe adjuntar un solo comprobante donde dice adjuntar comprobante y dejar vacío donde dice posterior.");
                            return View(model);
                        }
                    }
                    if(attch1 != null && attch2 == null) 
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attachment.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                        request.AttachmentPath = uniqueFileName;
                    } else if (attch1 != null && attch2 != null)
                    {*/

                    if (model.Attachment != null && model.Attachment2 != null)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                        uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attachment.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        model.Attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                        request.AttachmentPath = uniqueFileName;

                        uniqueFileName2 = Guid.NewGuid().ToString() + "_" + model.Attachment2.FileName;
                        string filePath2 = Path.Combine(uploadsFolder, uniqueFileName2);
                        model.Attachment2.CopyTo(new FileStream(filePath2, FileMode.Create));
                        request.AttachmentPath2 = uniqueFileName2;
                    } else
                    {
                        request.AttachmentPath = null;
                        request.AttachmentPath2 = null;
                    }
                    /*
                    } else
                    {
                        request.AttachmentPath = null;
                        request.AttachmentPath2 = null;
                    }*/

                    request.UserId = userId;
                    request.RequestDate = DateTime.Now;
                    request.BeginDate = model.BeginDate;
                    request.EndDate = model.EndDate;                    
                    request.Type = (RequestType)model.SelectedRequestType;

                    if (requestManager.Create(request))
                    {
                        return RedirectToAction("MyRequests");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error. Intente nuevamente revisando los datos ingresados");
                    }
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult MyRequests(int? page)
        {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Request> userRequests = requestManager.GetRequestsByUserId(userId);
                return View(userRequests.ToPagedList(page ?? 1, 10));            
        }


        public IActionResult DeleteRequest(int id)
        {
            Request request = requestManager.Find(id);

            if (request == null)
            {
                ViewBag.ErrorTitle = "Ocurrió un error";
                ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                return View("Error");
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
        public async Task<IActionResult> ManageSubordinatesRequests(string search, int? page)
        {
            List<ManageSubordinatesRequestsViewModel> model = new List<ManageSubordinatesRequestsViewModel>();
            List<ManageSubordinatesRequestsViewModel> aux;

            string loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            AppUser loggedUser = await userManager.FindByIdAsync(loggedUserId);
            int loggedUserDocument = loggedUser.Document;           

            List<Request> subordinatesRequests = GetSubordinateRequests(loggedUserDocument);

            aux = await RequestsToViewModel(subordinatesRequests);

            Filter(search, page, model, aux);

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ManageAllRequests(string search, int? page)
        {
            List<ManageSubordinatesRequestsViewModel> model = new List<ManageSubordinatesRequestsViewModel>();
            List<ManageSubordinatesRequestsViewModel> aux;
            List<Request> requests = requestManager.GetRequests();            

            aux = await RequestsToViewModel(requests);

            Filter(search, page, model, aux); 

            return View(); 
        }

        public IActionResult AcceptSubordinateRequest(int id)
        {
            requestManager.AcceptBySupervisor(id);
            return RedirectToAction("ManageSubordinatesRequests");
        }

        public async Task<IActionResult> RejectSubordinateRequest(int id)
        {
            try
            {
                Request r = requestManager.Find(id);
                if (r != null)
                {
                    requestManager.Reject(id);
                    var user = await userManager.FindByIdAsync(r.UserId);

                    // Notificar al mail
                    Notification noti = new Notification
                    {
                        To = user.Email,
                        Subject = "Su solicitud ha sido rechazada",
                        Body = NotificationController.GetBodyForEmail(false, user, r)
                    };

                    if (NotificationController.SendEmail(noti)) { return RedirectToAction("ManageSubordinatesRequests"); }
                    else
                    {
                        ViewBag.ErrorTitle = "Ocurrió un error";
                        ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                        return View("Error");
                    }
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Ocurrió un error";
                ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                return View("Error");
            }
            return RedirectToAction("ManageSubordinatesRequests");
        }

        public async Task<IActionResult> AcceptRequest(int id)
        {
            try
            {
                Request r = requestManager.Find(id);
                if (r != null)
                {
                    requestManager.Accept(id);
                    var user = await userManager.FindByIdAsync(r.UserId);

                    // Notificar al mail
                    Notification noti = new Notification
                    {
                        To = user.Email,
                        Subject = "Su solicitud ha sido aceptada",
                        Body = NotificationController.GetBodyForEmail(true, user, r)
                    };

                    if (NotificationController.SendEmail(noti)) { return RedirectToAction("ManageAllRequests"); } 
                    else
                    {
                        ViewBag.ErrorTitle = "Ocurrió un error";
                        ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                        return View("Error");
                    }                    
                }
            } catch
            {
                ViewBag.ErrorTitle = "Ocurrió un error";
                ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                return View("Error");
            }
            
            return RedirectToAction("ManageAllRequests");
        }

        public async Task<IActionResult> RejectRequest(int id)
        {
            try
            {
                Request r = requestManager.Find(id);
                if (r != null)
                {
                    requestManager.Reject(id);
                    var user = await userManager.FindByIdAsync(r.UserId);

                    // Notificar al mail
                    Notification noti = new Notification
                    {
                        To = user.Email,
                        Subject = "Su solicitud ha sido rechazada",
                        Body = NotificationController.GetBodyForEmail(false, user, r)
                    };

                    if (NotificationController.SendEmail(noti)) { return RedirectToAction("ManageAllRequests"); }
                    else
                    {
                        ViewBag.ErrorTitle = "Ocurrió un error";
                        ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                        return View("Error");
                    }
                }
            }
            catch
            {
                ViewBag.ErrorTitle = "Ocurrió un error";
                ViewBag.ErrorMessage = $"La solicitud con ID = {id} no fue encontrada";
                return View("Error");
            }

            return RedirectToAction("ManageAllRequests");
        }


        #endregion


        #region Metodos auxiliares


        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public ActionResult TenWorkingDaysFromToday([Bind(Prefix = "BeginDate")] string aDate)
        {

            try
            {
                var userdate = DateTime.Parse(aDate);
                var tenWorkingDaysFromToday = DateTime.Today;
                var tempDate = DateTime.Today;
                while(tempDate <= DateTime.Today.AddDays(14))
                {
                    if(tempDate.DayOfWeek == DayOfWeek.Saturday && tempDate.DayOfWeek == DayOfWeek.Sunday)
                    {                        
                        tenWorkingDaysFromToday = tenWorkingDaysFromToday.AddDays(1);
                    }
                    tenWorkingDaysFromToday = tenWorkingDaysFromToday.AddDays(1);
                    tempDate = tempDate.AddDays(1);
                }

                if (userdate >= tenWorkingDaysFromToday) 
                { 
                    return Json(true); 
                }
                else { return Json("Debe seleccionar una fecha a partir de los próximos 10 días"); }
            } catch
            {
                return Json("Datos inválidos");
            }
        }

        public ActionResult DeleteFilterAllRequests()
        {
            searchTemp = "";
            return RedirectToAction("ManageAllRequests");
        }

        public ActionResult DeleteFilterManageSubordinatesRequests()
        {
            searchTemp = "";
            return RedirectToAction("ManageSubordinatesRequests");
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

        private async Task<List<ManageSubordinatesRequestsViewModel>> RequestsToViewModel(List<Request> requests)
        {
            List<ManageSubordinatesRequestsViewModel> returnList = new List<ManageSubordinatesRequestsViewModel>();
            if (requests.Count > 0)
            {                
                foreach (Request r in requests)
                {
                    AppUser userRequesting = await userManager.FindByIdAsync(r.UserId);
                    ManageSubordinatesRequestsViewModel m = new ManageSubordinatesRequestsViewModel
                    {
                        Id = r.Id,
                        UserRequesting = userRequesting.Name + " " + userRequesting.Lastname,
                        Type = r.Type,
                        BeginDate = r.BeginDate,
                        EndDate = r.EndDate,
                        RequestDate = r.RequestDate,
                        Status = r.Status
                    };
                    returnList.Add(m);
                    returnList.Sort();
                }
            }

            return returnList;
        }
        
        private ActionResult Filter(string search, int? page, List<ManageSubordinatesRequestsViewModel> model, List<ManageSubordinatesRequestsViewModel> aux)
        {
            string searchLower;
            ViewBag.searchFilter = search;

            if (String.IsNullOrEmpty(search)) // Si search es null se está moviendo de página
            {
                if (!String.IsNullOrEmpty(searchTemp)) // Si searchTemp no es null, se está moviendo de página con filtro 
                {

                    searchLower = searchTemp;
                    ViewBag.searchFilter = searchTemp;

                    foreach (ManageSubordinatesRequestsViewModel a in aux)
                    {
                        string aLower = a.UserRequesting.ToLower();
                        if (aLower.Contains(searchLower))
                        {
                            model.Add(a);
                        }
                    }
                    return View(model.ToPagedList(page ?? 1, 5));

                }
                else // Se está moviendo de página sin filtro
                {
                    return View(aux.ToPagedList(page ?? 1, 5));
                }

            }
            else // está filtrando con un nombre nuevo
            {
                searchLower = search.ToLower();
                searchTemp = searchLower;

                foreach (ManageSubordinatesRequestsViewModel a in aux)
                {
                    string aLower = a.UserRequesting.ToLower();
                    if (aLower.Contains(searchLower))
                    {
                        model.Add(a);
                    }
                }
                return View(model.ToPagedList(page ?? 1, 5));
            }
        }
               

        #endregion

    }
}