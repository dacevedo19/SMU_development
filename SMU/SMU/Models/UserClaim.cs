using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class UserClaim : IComparable<UserClaim>
    {

        #region Atributos

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Empleado")]
        public string UserId { get; set; }
                
        [DisplayName("Texto")]
        public string Text { get; set; }

        [DisplayName("Fecha")]
        public DateTime ClaimDate { get; set; }

        [DisplayName("Solicitada el")]
        public DateTime SentClaimDate { get; set; }

        [DisplayName("Adjunto")]
        public string AttachmentPath { get; set; }

        #endregion

        #region Metodos

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is UserClaim a)) return false;
            if (this.Id == a.Id) return true;
            else return false;

        }

        public int SortByDate(DateTime claimDate1, DateTime claimDate2)
        {
            return claimDate1.CompareTo(claimDate2);
        }

        int IComparable<UserClaim>.CompareTo(UserClaim claim)
        {
            if (claim == null) { return 0; }
            else
            {
                return claim.ClaimDate.CompareTo(this.ClaimDate);
            }
        }

        #endregion

    }
}
