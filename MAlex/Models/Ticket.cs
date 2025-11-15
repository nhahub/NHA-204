using MetroApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAlex.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;

        // Foreign key to Trip
        [Required]
        public int TripID { get; set; }

        // Navigation property
        [ForeignKey("TripID")]
        public virtual Trip Trip { get; set; } = null!;

        // Many-to-many with Users through UserTicket junction table
        public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
    }
}

