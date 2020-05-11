using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class UserRepositary : IUserRepositary
    {
        private readonly AppDbContext context;

        public UserRepositary(AppDbContext context)
        {
            this.context = context;
        }

        #region CRUD

        public AppUser Add(AppUser user)
        {
            context.AppUsers.Add(user);
            context.SaveChanges();
            return user;
        }

        public AppUser GetUser(int id)
        {
            return context.AppUsers.Find(id);
        }

        public AppUser GetUserByDocument(string document)
        {
            return context.AppUsers.Find(document);
        }

        public IEnumerable<AppUser> GetUsers()
        {
            return context.AppUsers;
        }

        public AppUser Update(AppUser changedUser)
        {
            var user = context.AppUsers.Attach(changedUser);
            user.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return changedUser;
        }

        public AppUser Delete(int id)
        {
            AppUser usuario = context.AppUsers.Find(id);
            if (usuario != null)
            {
                context.Users.Remove(usuario);
                context.SaveChanges();
            }
            return usuario;
        }

        #endregion


    }
}