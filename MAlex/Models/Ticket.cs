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
        [Required]
        public int TripID { get; set; }

    
        [ForeignKey("TripID")]
        public virtual Trip Trip { get; set; } = null!;

    
        public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
    }
}

