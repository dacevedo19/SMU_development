using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SMU.Models;
using SMU.ViewModels;
using X.PagedList;

namespace SMU.Controllers
{
    [Authorize(Roles = "SuperAdmin,RecursosHumanos")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        static string searchTemp = "";

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        #region Metodos

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
            if (role == null)
            {
                ViewBag.ErrorMessage = $"El rol con ID = {role.Id} no fue encontrado";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
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
        public IActionResult ListUsers(string search, int? page)
        {
            List<AppUser> model = new List<AppUser>();
            List<AppUser> aux;
            aux = userManager.Users.ToList();

            Filter(search, page, model, aux);
            
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            List<IdentityRole> allRoles = new List<IdentityRole>();
            allRoles = roleManager.Roles.ToList();

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID = {user.Id} no fue encontrado";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Supervisor = GetSupervisorNameById(user.Supervisor),
                Document = user.Document,
                Email = user.Email,
                Name = user.Name,
                Lastname = user.Lastname,
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
                int supervisor = GetSupervisorIdByName(model.Supervisor);
                if (supervisor == 0)
                {
                    ModelState.AddModelError("", "No se encontró ningún supervisor con este nombre");
                    List<IdentityRole> allRoles = new List<IdentityRole>();
                    allRoles = roleManager.Roles.ToList();
                    model.RolesList = allRoles.Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    });
                }
                else
                {
                    user.Supervisor = supervisor;
                    user.Document = model.Document;
                    user.Name = model.Name;
                    user.Lastname = model.Lastname;
                    user.EntryDate = model.EntryDate;
                    user.Active = model.Active;

                    //Actualizar el rol                    
                    var userRoles = await userManager.GetRolesAsync(user);
                    var previousRole = userRoles.FirstOrDefault();
                    if (previousRole != null)
                    {
                        await userManager.RemoveFromRoleAsync(user, previousRole);
                        if (selectedRole != null) { await userManager.AddToRoleAsync(user, selectedRole.Name); }
                        else { await userManager.AddToRoleAsync(user, previousRole); }
                    }
                    else
                    {
                        if (selectedRole != null) { await userManager.AddToRoleAsync(user, selectedRole.Name); }
                        else { await userManager.AddToRoleAsync(user, "Empleado"); }
                    }
                        


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
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"El usuario con ID = {id} no fue encontrado";
                return View("NotFound");
            }
            else
            {
                user.Active = false;
                var result = await userManager.UpdateAsync(user);

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


        #endregion


        #region Metodos auxiliares

        private int GetSupervisorIdByName(string supName)
        {
            if (!String.IsNullOrEmpty(supName))
            {
                string supervisor = supName.ToLower();
                var users = userManager.Users;
                foreach (AppUser u in users)
                {
                    string userName = u.Name + " " + u.Lastname;
                    if (userName.ToLower().Equals(supervisor))
                    {
                        return u.Document;
                    }
                }
            }
            return 0;
        }

        private string GetSupervisorNameById(int document)
        {
            var users = userManager.Users;
            foreach (AppUser u in users)
            {
                if (u.Document == document)
                {
                    return u.Name + " " + u.Lastname;
                }
            }
            return " ";
        }

        private ActionResult Filter(string search, int? page, List<AppUser> model, List<AppUser> aux)
        {
            string searchLower;
            ViewBag.searchFilter = search;

            if (String.IsNullOrEmpty(search)) // Si search es null se está moviendo de página
            {
                if (!String.IsNullOrEmpty(searchTemp)) // Si searchTemp no es null, se está moviendo de página con filtro 
                {
                    searchLower = searchTemp;
                    ViewBag.searchFilter = searchTemp;

                    foreach (AppUser a in aux)
                    {
                        string fullName = a.Name + " " + a.Lastname;
                        string fullNameLower = fullName.ToLower();
                        if (fullNameLower.Contains(searchLower))
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

                foreach (AppUser a in aux)
                {
                    string fullName = a.Name + " " + a.Lastname;
                    string fullNameLower = fullName.ToLower();
                    if (fullNameLower.Contains(searchLower))
                    {
                        model.Add(a);
                    }
                }
                return View(model.ToPagedList(page ?? 1, 5));
            }
        }

        public ActionResult DeleteFilter()
        {
            searchTemp = "";
            return RedirectToAction("ListUsers");
        }

        #endregion


    }
}