﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMU.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMU.ViewModels
{
    [NotMapped]
    public class RegisterRequestViewModel
    {

        #region Atributos

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Tipo de licencia")]
        public int SelectedRequestType { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public RequestType Type { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Desde")]
        [Remote(action: "TenWorkingDaysFromToday", controller: "Request", ErrorMessage = "La fecha debe ser en el futuro")]
        public DateTime BeginDate { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Hasta")]
        public DateTime EndDate { get; set; }

        [DisplayName("Adjuntar archivo")]
        public IFormFile Attachment { get; set; }

        public SelectList ListOfTypes { get; set; }

        #endregion

    }
}
