using MetroApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace MAlex.Models
{
    public class Subscrubtion
    {
        [Key]
        public int SubscriptionID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty; // e.g. Monthly, Quarterly

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        // Duration in days (30 = monthly, 90 = quarterly)
        [Required]
        public int DurationDays { get; set; }

        public string? Description { get; set; }

        // Navigation
        public ICollection<UserSubscription>? UserSubscriptions { get; set; }
    }
}