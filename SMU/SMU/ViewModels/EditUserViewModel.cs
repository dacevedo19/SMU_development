using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMU.ViewModels
{
    [NotMapped]
    public class EditUserViewModel
    {

        #region Atributos
        public string Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [ForeignKey("AppUser")]
        [Display(Name = "Nombre de su supervisor")]
        public string Supervisor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1000000, 300000000, ErrorMessage = "Número de documento inválido")]
        [Display(Name = "Cédula de identidad")]
        public int Document { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de entrada")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Rol")]
        public string SelectedRole { get; set; }

        public string Role { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }


        #endregion

    }
}
