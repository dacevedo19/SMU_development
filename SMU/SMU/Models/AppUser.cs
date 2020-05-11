using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class AppUser : IdentityUser
    {

        #region Atributos

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [ForeignKey("AppUser")]
        [Display(Name = "ID Supervisor")]
        public int IdSupervisor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(10, ErrorMessage = "Número invalido", MinimumLength = 7)]
        public string Document { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30)]
        public String Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30)]
        public String Lastname { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de entrada")]
        public DateTime EntryDate { get; set; }

        public List<Request> RequestList { get; set; }

        public List<License> LicenseList { get; set; }


        #endregion

        #region Constructores

        //public AppUser() {}

        #endregion
    }
}
