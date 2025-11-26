using MAlex.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MetroApp.Models
{
    public class User : IdentityUser
    {
       


        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(20)]
        public string? Role { get; set; }

        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiry { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now; 
public virtual ICollection<Subscrubtion> Subscriptions { get; set; } = new List<Subscrubtion>();
        public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
    }
}