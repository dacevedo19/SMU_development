using Microsoft.AspNetCore.Http;
using SMU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    public class RegisterClaimViewModel
    {

        #region Atributos

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Empleado")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [MaxLength(30, ErrorMessage = "El texto es muy largo. Trate de acortarlo e intente nuevamente")]
        [DisplayName("Texto")]
        public string Text { get; set; }

        [DisplayName("Fecha del hecho")]
        public DateTime ClaimDate { get; set; }

        [DisplayName("Registrada el")]
        public DateTime SentClaimDate { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        [DisplayName("Adjuntar archivo")]
        public IFormFile Attachment { get; set; }

        #endregion


    }
}
