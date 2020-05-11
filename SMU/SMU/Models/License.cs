using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class License
    {
        #region Atributos

        public int Id { get; set; }

        public int UserId { get; set; }

        public Request RequestMade { get; set; }

        public enum RequestType { };

        public DateTime RequestDate { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public Byte[] Attachment { get; set; }

        #endregion

        #region Constructores

        public License() { }

        #endregion
    }
}
