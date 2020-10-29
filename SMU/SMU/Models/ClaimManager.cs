using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class ClaimManager : IClaimManager
    {

        private readonly AppDbContext context;

        public ClaimManager(AppDbContext context)
        {
            this.context = context;
        }

        public bool Create(UserClaim claim)
        {
            if (claim == null) { return false; }
            try
            {
                context.Claims.Add(claim);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Delete(int claimId)
        {
            UserClaim claim = context.Claims.Find(claimId);
            if (claim == null) { return false; }
            try
            {
                context.Claims.Remove(claim);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Update(UserClaim claim)
        {
            try
            {
                var c = context.Claims.Attach(claim);
                c.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public UserClaim Find(int claimId)
        {
            return context.Claims.Find(claimId);
        }

        public List<UserClaim> GetClaims()
        {
            return context.Claims.ToList();
        }

        public List<UserClaim> GetClaimsByUserId(string id)
        {
            List<UserClaim> claims = context.Claims.ToList();
            List<UserClaim> userClaims = new List<UserClaim>();

            foreach (UserClaim c in claims)
            {
                if (c.UserId == id) { userClaims.Add(c); }
            }
            return userClaims;
        }

        
    }
}
