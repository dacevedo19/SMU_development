using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMU.Models;
using SMU.ViewModels;

namespace SMU.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }

        public UserManager<AppUser> UserManager { get; }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { 
                    UserName = model.Email, Email = model.Email, Document = model.Document, Name = model.Name,
                    Lastname = model.Lastname, Supervisor = model.Supervisor, EntryDate = model.EntryDate 
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    logger.Log(LogLevel.Warning, confirmationLink);


                    if(signInManager.IsSignedIn(User) && User.IsInRole("SuperAdmin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    //descomentar esto despues
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("Index", "Home");

                    ViewBag.ErrorTitle = "Se ha registrado exitosamente.";
                    ViewBag.ErrorMessage = "Antes de ingresar deberá confirmar su email. Por favor haga click en el link que se ha enviado a su casilla.";
                    return View("Error");

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //Quitar comentarios para activar la función confirmar email para loguearse.
                /*var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && !user.EmailConfirmed
                    && (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError("", "El email no fue confirmado. Revise su casilla de correo para confirmar su email.");
                    return View(model);
                }
                */
                

                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "El usuario o la contraseña son incorrectos");

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (User == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await UserManager.FindByIdAsync(userId);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID {userId} no existe";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "El email no fue confirmado";
            return View("Error");
        }


        [HttpPost]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"La casilla {email} está en uso.");
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {                
                var user = await userManager.FindByEmailAsync(model.Email);
                
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {                    
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                                        
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token = token }, Request.Scheme);
                                       
                    logger.Log(LogLevel.Warning, passwordResetLink);                   
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Token de restauración de contraseña inválido");
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {                
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {                    
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                
                return View("ResetPasswordConfirmation");
            }
            
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

    }
}