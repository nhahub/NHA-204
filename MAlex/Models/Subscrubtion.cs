using MetroApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAlex.Models
{
    public class Subscrubtion
    {
        [Key]
        public int SubscriptionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } = string.Empty; 

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; 

        [Required]
        public string UserID { get; set; } = string.Empty;

        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;
    }
}