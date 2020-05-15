using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    [NotMapped]
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede contener más de 30 caracteres.")]
        public string RoleName { get; set; }
    }
}
