using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string  Email { get; set; }

    }
}
