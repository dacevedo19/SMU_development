using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    [NotMapped]
    public class SeedData : ISeedData
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        
        //Constructor
        public SeedData(AppDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Método principal
        public async Task Seed()
        {
            //Actualiza la db si existen migraciones pendientes
            if(_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }

            //Creación de roles
            #region Roles

            string[] roles = new string[] { "SuperAdmin", "RecursosHumanos", "Supervisor", "Empleado", "Test1" };

            foreach (string role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(_db);

                if (!_db.Roles.Any(r => r.Name == role))
                {
                    //_db.Roles.Add(new IdentityRole(role));
                    IdentityRole Irole = new IdentityRole() { Name = role };
                    IdentityResult result = await _roleManager.CreateAsync(Irole);
                }
            }


            #endregion

            //Creación de usuarios
            #region Users
            List<AppUser> users = new List<AppUser>();

            AppUser u1 = new AppUser
            {
                UserName = "superadmin@gmail.com",                
                Email = "superadmin@gmail.com",
                Document = 77777777,
                Name = "Super",
                Lastname = "Admin",
                Supervisor = 77777777,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u2 = new AppUser
            {
                UserName = "recursoshumanos1@gmail.com",
                Email = "recursoshumanos1@gmail.com",
                Document = 12355487,
                Name = "Ricky",
                Lastname = "McPherson",
                Supervisor = 12355487,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u3 = new AppUser
            {
                UserName = "supervisor1@gmail.com",
                Email = "supervisor1@gmail.com",
                Document = 25416638,
                Name = "Agustin",
                Lastname = "Bodder",
                Supervisor = 12355487,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u4 = new AppUser
            {
                UserName = "supervisor2@gmail.com",
                Email = "supervisor2@gmail.com",
                Document = 14423695,
                Name = "Alvaro",
                Lastname = "Rothson",
                Supervisor = 12355487,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u5 = new AppUser
            {
                UserName = "empleado1@gmail.com",
                Email = "empleado1@gmail.com",
                Document = 55612477,
                Name = "Carlos",
                Lastname = "Perez",
                Supervisor = 25416638,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u6 = new AppUser
            {
                UserName = "empleado2@gmail.com",
                Email = "empleado2@gmail.com",
                Document = 148721008,
                Name = "Juan",
                Lastname = "Gomez",
                Supervisor = 25416638,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u7 = new AppUser
            {
                UserName = "empleado3@gmail.com",
                Email = "empleado3@gmail.com",
                Document = 64714221,
                Name = "Martin",
                Lastname = "Perdomo",
                Supervisor = 14423695,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };
            AppUser u8 = new AppUser
            {
                UserName = "empleado4@gmail.com",
                Email = "empleado4@gmail.com",
                Document = 8741123,
                Name = "Agustin",
                Lastname = "Cajal",
                Supervisor = 14423695,
                EntryDate = System.DateTime.Today,
                EmailConfirmed = true
            };

            users.Add(u1);
            users.Add(u2);
            users.Add(u3);
            users.Add(u4);
            users.Add(u5);
            users.Add(u6);
            users.Add(u7);
            users.Add(u8);
                       
            foreach (AppUser user in users)
            {

                if (!_db.Users.Any(u => u.UserName == user.UserName))
                {
                    await _userManager.CreateAsync(user, "Password.123");                    
                }               
            }

            #endregion

            //Asignación de roles a usuarios
            #region Asignation

            await _userManager.AddToRoleAsync(u1, "SuperAdmin");
            await _userManager.UpdateAsync(u1);
            await _userManager.AddToRoleAsync(u2, "RecursosHumanos");
            await _userManager.UpdateAsync(u2);
            await _userManager.AddToRoleAsync(u3, "Supervisor");
            await _userManager.UpdateAsync(u3);
            await _userManager.AddToRoleAsync(u4, "Supervisor");
            await _userManager.UpdateAsync(u4);
            await _userManager.AddToRoleAsync(u5, "Empleado");
            await _userManager.UpdateAsync(u5);
            await _userManager.AddToRoleAsync(u6, "Empleado");
            await _userManager.UpdateAsync(u6);
            await _userManager.AddToRoleAsync(u7, "Empleado");
            await _userManager.UpdateAsync(u7);
            var result1 = await _userManager.AddToRoleAsync(u8, "Empleado");
            if (result1.Succeeded)
            {
                await _userManager.UpdateAsync(u8);
            }

            #endregion


        }

    }
}
