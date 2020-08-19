
using SMU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    public class ManageSubordinatesRequestsViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Empleado")]
        public string UserRequesting { get; set; }        

        [DisplayName("Tipo")]
        public RequestType Type { get; set; }        

        [DisplayName("Desde")]        
        public DateTime BeginDate { get; set; }
                
        [DisplayName("Hasta")]
        public DateTime EndDate { get; set; }

        [DisplayName("Solicitada el")]
        public DateTime RequestDate { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        

    }
}
