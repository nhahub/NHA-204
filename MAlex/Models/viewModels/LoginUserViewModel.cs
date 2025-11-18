using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class LoginUserViewModel
    {
        [Required(ErrorMessage ="*")]
        public string Username { get; set; }
   
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
        
    }
}
