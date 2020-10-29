using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public interface IClaimManager
    {
        public bool Create(UserClaim claim);
        public bool Update(UserClaim claim);
        public bool Delete(int claimId);
        public UserClaim Find(int claimId);
        public List<UserClaim> GetClaims();
        public List<UserClaim> GetClaimsByUserId(string id);

    }
}
