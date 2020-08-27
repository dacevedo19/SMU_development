using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SMU.ViewModels
{
    [NotMapped]
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }

        #region Atributos
        public string Id { get; set; }

        [Required(ErrorMessage ="Este campo es obligatorio")]
        public string Name { get; set; }

        public List<string> Users;

        #endregion  

    }
}
