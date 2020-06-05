using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Debe proporcionar un mail")]
        [EmailAddress(ErrorMessage = "El formato no es válido")]
        public string Email { get; set; }
    }
}
