using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    interface IUserRepositary
    {
        AppUser GetUser(int id);
        AppUser GetUserByDocument(string document);
        IEnumerable<AppUser> GetUsers();
        AppUser Add(AppUser user);
        AppUser Update(AppUser changedUser);
        AppUser Delete(int id);
    }
}
