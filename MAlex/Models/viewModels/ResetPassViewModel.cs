using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class ResetPassViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword{ get; set; }


    }
}
