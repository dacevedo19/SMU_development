using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SMU.Models
{
    public enum Status
    {
        [Description("En proceso")]
        Issued,
        [Description("Aceptada")]
        Accepted,
        [Description("Rechazada")]
        Declined
    }
}
