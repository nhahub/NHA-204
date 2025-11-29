using MetroApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAlex.Models
{
    public class UserSubscription
    {
        [Key]
        public int UserSubscriptionID { get; set; }

        [Required]
        public string UserID { get; set; } = string.Empty;

        [ForeignKey(nameof(UserID))]
        public virtual User? User { get; set; }   // Your User entity (Identity or custom)

        [Required]
        public int SubscriptionID { get; set; }

        [ForeignKey(nameof(SubscriptionID))]
        public virtual Subscrubtion? Subscription { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }     // computed when creating

        [Required]
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}