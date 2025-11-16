using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAlex.Models
{
    public class Trip
    {
        [Key]
        public int TripID { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Distance { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        public string Type { get; set; } = "Regular";  

        
        [Required]
        public int StartStationID { get; set; }

        [Required]
        public int EndStationID { get; set; }

       
        [ForeignKey("StartStationID")]
        public virtual Station StartStation { get; set; } = null!;

        [ForeignKey("EndStationID")]
        public virtual Station EndStation { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


       
      
        public override string ToString()
        {
            return $"{StartStation?.StationName} → {EndStation?.StationName} | {Type} | {TotalPrice} EGP";
        }
    }
}
