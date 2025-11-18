using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class RegisterViewModel
    {
        public string Username { get; set; }

        [DataType(DataType.Password)]
         public string Password { get; set; }

        [Compare("Password")]
        [Display(Name =("Comfirm Password"))]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } 

    }
}
