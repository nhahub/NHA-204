using System.ComponentModel.DataAnnotations;

namespace MAlex.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? ExpiresAt { get; set; }

        public bool IsActive { get; set; } = true;

        public NotificationType Type { get; set; } = NotificationType.General;

        // Foreign key for admin who created it
        public string? CreatedByUserId { get; set; }
    }

    public enum NotificationType
    {
        General,
        ServiceUpdate,
        Maintenance,
        Emergency,
        Promotion
    }
}



