using System;
using System.ComponentModel.DataAnnotations;

namespace MAlex.ViewModels
{
    public class PurchaseSubscriptionViewModel
    {
        [Required]
        public int SubscriptionID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        public decimal Price { get; set; } // filled from subscription
        public string? SubscriptionName { get; set; }
    }
}
