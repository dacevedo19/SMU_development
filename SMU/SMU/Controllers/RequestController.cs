using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public RequestController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IRequestManager requestManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.requestManager = requestManager;
        }


        [HttpGet]
        public IActionResult RegisterRequest()
        {
            var model = new RegisterRequestViewModel();
            var types = GetRequestTypes();
            model.TypesList = types.Select(t => new SelectListItem
            {
                Value = t.ToString(),
                Text = t.ToString()
            });

            return View(model);
        }

        [HttpPost]
        public IActionResult RegisterRequest(RegisterRequestViewModel model)
        {
            Request request = new Request();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            
            if(model != null)
            {                
                request.UserId = userId;
                request.RequestDate = DateTime.Today;
                request.BeginDate = model.BeginDate;
                request.EndDate = model.EndDate;
                request.Attachment = model.Attachment;
                request.Status = Status.Issued;
                switch (model.SelectedRequestType)
                {
                    case "Vacacional":
                        request.Type = RequestType.Vacacional;
                        break;
                    case "Médica":
                        request.Type = RequestType.Médica;
                        break;
                    case "Estudio":
                        request.Type = RequestType.Estudio;
                        break;
                    case null:
                        break;
                }

                if (requestManager.Create(request))
                {
                    return RedirectToAction("Index", "Home");
                } else
                {
                    ModelState.AddModelError("","Ocurrió un error. Intente nuevamente revisando los datos ingresados");
                }
            }

            return View(model);
        }


        
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

        private IEnumerable<RequestType> GetRequestTypes()
        {
            return Enum.GetValues(typeof(RequestType)).Cast<RequestType>();
        }


    }
}