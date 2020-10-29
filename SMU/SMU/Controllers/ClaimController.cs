using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SMU.Models;
using SMU.ViewModels;

namespace SMU.Controllers
{
    public class ClaimController : Controller
    {

        private readonly IClaimManager claimManager;
        private readonly IWebHostEnvironment hostingEnvironment;

        //CONSTRUCTOR
        public ClaimController(IClaimManager claimManager, IWebHostEnvironment hostingEnvironment)
        { 
            this.claimManager = claimManager;
            this.hostingEnvironment = hostingEnvironment;
        }

        #region Methods

        [HttpGet]
        public IActionResult RegisterClaim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterClaim(RegisterClaimViewModel model)
        {
            UserClaim claim = new UserClaim();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (model != null)
            {
                #region Attachment part

                string uniqueFileName; 
                  
                if (model.Attachment != null) 
                {                    
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attachment.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Attachment.CopyTo(new FileStream(filePath, FileMode.Create));
                    claim.AttachmentPath = uniqueFileName;

                }
                else 
                { claim.AttachmentPath = null; }



                #endregion

                claim.UserId = userId;
                claim.Text = model.Text;
                claim.ClaimDate = model.ClaimDate;
                claim.SentClaimDate = DateTime.Now;



                if (claimManager.Create(claim))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error. Intente nuevamente revisando los datos ingresados");
                }

                return View(model);
            }
            return View(model);


        }

        #endregion
    
    }
}
