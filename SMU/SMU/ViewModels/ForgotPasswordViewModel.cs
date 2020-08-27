using System.ComponentModel.DataAnnotations;


namespace SMU.ViewModels
{
    public class ForgotPasswordViewModel
    {

        #region Atributos

        [Required(ErrorMessage = "Debe proporcionar un mail")]
        [EmailAddress(ErrorMessage = "El formato no es válido")]
        public string Email { get; set; }

        #endregion

    }
}
