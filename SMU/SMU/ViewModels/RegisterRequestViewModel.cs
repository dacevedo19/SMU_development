using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class RegisterRequestViewModel
    {          
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Tipo de licencia")]
        public string SelectedRequestType { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string RequestType { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Desde")]
        [Remote(action: "IsDateInFuture", controller: "Request", ErrorMessage = "La fecha debe ser en el futuro")]
        public DateTime BeginDate { get; set; }
        
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DisplayName("Hasta")]        
        public DateTime EndDate { get; set; }

        [DisplayName("Adjuntar archivo")]
        public Byte[] Attachment { get; set; }

        public DateTime today = DateTime.Today;

        public IEnumerable<SelectListItem> TypesList { get; set; }
    }
}
