using System.ComponentModel.DataAnnotations;

namespace MAlex.Models.viewModels
{
    public class BookTicketViewModel
    {
        
        public string? TicketType { get; set; } = string.Empty;

        [Required]
        public int Price { get; set; }

        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = "Cash";

        [Required]
        [Display(Name = "From Station")]
        public string FromStation { get; set; } = string.Empty;

        [Required]
        [Display(Name = "To Station")]
        public string ToStation { get; set; } = string.Empty; 
    }
}

