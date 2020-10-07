using System;
using System.ComponentModel;


namespace SMU.Models
{
    public class Request : IComparable<Request>
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

        [DisplayName("Adjunto")]
        public string AttachmentPath2 { get; set; }

        #endregion

        #region Constructores
        public Request() { }

        #endregion

        #region Metodos

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is Request a)) return false;
            if (this.Id == a.Id) return true;
            else return false;

        }

        public int SortByDate(DateTime rqstDate1, DateTime rqstDate2)
        {
            return rqstDate1.CompareTo(rqstDate2);
        }

        int IComparable<Request>.CompareTo(Request rqst)
        {
            if (rqst == null) { return 0; }
            else
            {
                return rqst.RequestDate.CompareTo(this.RequestDate);
            }
        }


        #endregion

    }
}
