using System;
using System.ComponentModel;


namespace SMU.Models
{
    public class Request
    {

        #region Atributos

        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Empleado")]
        public string UserId { get; set; }

        [DisplayName("Tipo")]
        public RequestType Type { get; set; }

        [DisplayName("Solicitada el")]
        public DateTime RequestDate { get; set; }        

        [DisplayName("Desde")]
        public DateTime BeginDate { get; set; }

        [DisplayName("Hasta")]
        public DateTime EndDate { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        [DisplayName("Adjunto")]
        public string AttachmentPath { get; set; }

        #endregion

        #region Constructores
        public Request() { }

        #endregion
    
    }
}
