
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace SMU.Models
{    
    public enum Status
    {
        [Display(Name ="Procesada")]
        Procesada = 0,        

        [Display(Name = "Aceptada")]
        Aceptada = 1,

        [Display(Name = "Rechazada")] 
        Rechazada = 2,

        [Display(Name = "En RR.HH.")]
        EnRecursosHumanos = 3,


    }

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Status enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
    }
}
