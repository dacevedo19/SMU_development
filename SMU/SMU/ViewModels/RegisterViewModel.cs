﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    [NotMapped]
    public class RegisterViewModel
    {

        #region Atributos

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato no es válido")]
        [Remote(action: "IsEmailInUse", controller: "Account", ErrorMessage = "Este email ya está en uso")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DisplayName("Confirmar contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(1000000, 300000000, ErrorMessage = "Número de documento inválido")]
        [Display(Name = "Cédula de identidad")]
        public int Document { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Número de empleado")]
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [ForeignKey("AppUser")]
        [Display(Name = "Nombre de su supervisor")]
        public string Supervisor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "La cantidad máxima de caracteres es 30")]
        [Display(Name = "Apellido")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de entrada")]
        public DateTime EntryDate { get; set; }

        #endregion

    }
}
