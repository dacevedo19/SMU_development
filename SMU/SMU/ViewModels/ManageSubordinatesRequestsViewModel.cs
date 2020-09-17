
using SMU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.ViewModels
{
    public class ManageSubordinatesRequestsViewModel : IComparable<ManageSubordinatesRequestsViewModel>
    {

        #region Atributos

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

        [DisplayName("Buscar por nombre")]
        public string SearchByName { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ManageSubordinatesRequestsViewModel a)) return false;
            if (this.Id == a.Id) return true;
            else return false;
                    
        }

        public int SortByDate(DateTime rqstDate1, DateTime rqstDate2)
        {
            return rqstDate1.CompareTo(rqstDate2);
        }

        int IComparable<ManageSubordinatesRequestsViewModel>.CompareTo(ManageSubordinatesRequestsViewModel rqst)
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
