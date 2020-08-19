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
        
        [Display(Name = "Cedula del supervisor")]
        public int Supervisor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1000000, 300000000, ErrorMessage = "Número de documento inválido")]
        [Display(Name = "Cédula de identidad")]
        public int Document { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de entrada")]
        public DateTime EntryDate { get; set; }

        public List<Request> RequestList { get; set; }    


        #endregion

        #region Constructores

        //public AppUser() {}

        #endregion
    }
}
