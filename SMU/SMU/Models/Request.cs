using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public class Request
    {
        #region Atributos

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public enum Status { };
        public enum RequestType { };
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Byte[] Attachment { get; set; }

        #endregion

        #region Constructores

        public Request() { }

        #endregion
    }
}
