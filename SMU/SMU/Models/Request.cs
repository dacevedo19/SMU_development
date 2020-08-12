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
        public string UserId { get; set; }
        public DateTime RequestDate { get; set; }
        public Status Status { get; set; }
        public RequestType Type { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Byte[] Attachment { get; set; }

        #endregion

        #region Constructores

        public Request() { }

        #endregion
    }
}
