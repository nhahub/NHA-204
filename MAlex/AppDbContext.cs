using MAlex.Models;
using MetroApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MAlex
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Subscrubtion> Subscriptions { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<UserTicket> UserTickets { get; set; }
        public DbSet<TicketType> TicketTypes { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TicketType>().HasData(
                  new TicketType { Id = 1, Type = "Single Ride", Price = 15, Description = "Valid for one trip between any two stations." },
                  new TicketType { Id = 2, Type = "Daily Pass", Price = 40, Description = "Unlimited rides for 24 hours." },
                  new TicketType { Id = 3, Type = "Weekly Pass", Price = 100, Description = "Unlimited rides for 7 days." }
             );
         
            builder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.TripID);
                entity.Property(e => e.Distance).HasPrecision(6, 2);
                entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

                entity.HasOne(t => t.StartStation)
                      .WithMany(s => s.StartTrips)
                      .HasForeignKey(t => t.StartStationID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.EndStation)
                      .WithMany(s => s.EndTrips)
                      .HasForeignKey(t => t.EndStationID)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.StartStationID);
                entity.HasIndex(e => e.EndStationID);
            });

            builder.Entity<Ticket>(entity =>
            {
              
                entity.HasKey(e => e.TicketID);
                entity.HasIndex(e => e.TripID);
            });

            
            builder.Entity<UserTicket>(entity =>
            {
                entity.HasKey(e => new { e.UserID, e.TicketID });
                entity.HasIndex(e => e.UserID);
                entity.HasIndex(e => e.TicketID);
            });

            // Configure Subscription
            builder.Entity<Subscrubtion>(entity =>
            {
                entity.HasKey(e => e.SubscriptionID);
                entity.HasIndex(e => e.UserID);
                entity.Property(e => e.Price).HasPrecision(10, 2);
            });

           
            builder.Entity<Station>(entity =>
            {
                entity.HasKey(e => e.StationID);
                entity.HasIndex(e => e.StationName);

                entity.HasData(
                    new Station { StationID = 1, StationName = "Raml", Location = "Raml Square", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 2, StationName = "Mahattet El Raml", Location = "El Raml District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 3, StationName = "Cleopatra", Location = "Cleopatra District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 4, StationName = "Sidi Gaber", Location = "Sidi Gaber Area", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 5, StationName = "Sporting", Location = "Sporting District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 6, StationName = "Ibrahimia", Location = "Ibrahimia Area", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 7, StationName = "Azarita", Location = "Azarita District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 8, StationName = "Shatby", Location = "Shatby Main Road", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 9, StationName = "Smouha", Location = "Smouha Square", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 10, StationName = "Kafr Abdou", Location = "Kafr Abdou Street", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 11, StationName = "Victoria", Location = "Victoria District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 12, StationName = "Mandara", Location = "Mandara Area", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 13, StationName = "Asafra", Location = "Asafra Main Street", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 30, 0), Status = "Active" },
                    new Station { StationID = 14, StationName = "Miami", Location = "Miami Corniche", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 15, StationName = "Louran", Location = "Louran District", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 16, StationName = "Sidi Bishr", Location = "Sidi Bishr Area", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 17, StationName = "Al Asafra Beach", Location = "Asafra Beach Road", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 18, StationName = "Montaza", Location = "Montaza Gardens", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(23, 0, 0), Status = "Active" },
                    new Station { StationID = 19, StationName = "Stanley", Location = "Stanley Bridge", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" },
                    new Station { StationID = 20, StationName = "Gleem", Location = "Gleem Area", OpeningTime = new TimeSpan(6, 0, 0), ClosingTime = new TimeSpan(22, 0, 0), Status = "Active" }
                );
            });

        }
    }
}
