using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMU.ViewModels
{
    [NotMapped]
    public class CreateRoleViewModel
    {

        #region Atributos

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [StringLength(30, ErrorMessage = "El nombre no puede contener más de 30 caracteres.")]
        public string RoleName { get; set; }

        #endregion

    }
}
