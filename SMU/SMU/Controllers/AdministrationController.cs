using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SMU.Models;
using SMU.ViewModels;

namespace SMU.Controllers
{
    [Authorize(Roles = "SuperAdmin,RecursosHumanos")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole() { Name = model.RoleName };
                IdentityResult result = await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {id} no fue encontrado";
                return View("NotFound");
            }
            var model = new EditRoleViewModel { Id = role.Id, Name = role.Name };
            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {model.Id} no fue encontrado";
                return View("NotFound");
            }
            else
            {
                role.Name = model.Name;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {id} no fue encontrado";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                } 
                catch (DbUpdateException)
                {
                    ViewBag.ErrorTitle = $"El rol {role.Name} está en uso.";
                    ViewBag.ErrorMessage = $"El rol {role.Name} tiene usuarios asignados. " +
                        $"Debe quitarlos de este rol para poder borrar el rol.";
                    return View("Error");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {role.Id} no fue encontrado";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel { UserId = user.Id, UserName = user.UserName };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }

            ViewBag.RoleName = role.Name;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {role.Id} no fue encontrado";
                return View("NotFound");
            }
            for(int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                    {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            List<IdentityRole> allRoles = new List<IdentityRole>();
            allRoles = roleManager.Roles.ToList();

            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID = {user.Id} no fue encontrado";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();

            var model = new EditUserViewModel {
                Id = user.Id,
                Supervisor = user.Supervisor,
                Document = user.Document,
                Email = user.Email,
                Name = user.Name, Lastname = user.Lastname,
                EntryDate = user.EntryDate,
                Role = userRole   
            };
            model.RolesList = allRoles.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            });            
            return View(model);
        }       

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var selectedRole = await roleManager.FindByIdAsync(model.SelectedRole);
            var user = await userManager.FindByIdAsync(model.Id);            

            if (user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID = {model.Id} no fue encontrado";
                return View("NotFound");
            } 
            else
            {                
                user.Supervisor = model.Supervisor;
                user.Document = model.Document;
                user.Name = model.Name;
                user.Lastname = model.Lastname;
                user.EntryDate = model.EntryDate;

                //Actualizar el rol
                var userRoles = await userManager.GetRolesAsync(user);
                var previousRole = userRoles.FirstOrDefault();
                if(previousRole != null) { await userManager.RemoveFromRoleAsync(user, previousRole); }
                await userManager.AddToRoleAsync(user, selectedRole.Name);


                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                } 
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }         
            return View(model);
        }
        
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID = {id} no fue encontrado";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }
        }


    }
}