using System.ComponentModel.DataAnnotations;

namespace MAlex.Models
{
    public class Station
    {
        [Key]
        public int StationID { get; set; }

        [Required]
        [StringLength(100)]
        public string StationName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        public TimeSpan ClosingTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

      
        public virtual ICollection<Trip> StartTrips { get; set; } = new List<Trip>();
        public virtual ICollection<Trip> EndTrips { get; set; } = new List<Trip>();
    }
}

