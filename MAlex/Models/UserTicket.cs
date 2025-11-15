using MAlex.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MetroApp.Models
{
    public class UserTicket
    {
        [Key, Column(Order = 0)]
        public string UserID { get; set; } = string.Empty;

        [Key, Column(Order = 1)]
        public int TicketID { get; set; }

        // Navigation properties
        [ForeignKey("UserID")]
        public virtual User User { get; set; } = null!;

        [ForeignKey("TicketID")]
        public virtual Ticket Ticket { get; set; } = null!;
    }
}



