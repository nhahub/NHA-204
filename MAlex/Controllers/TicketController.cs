using MAlex.Models;
using MAlex.Models.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MAlex.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

        public TicketController(AppDbContext context)
        {
            _context = context;
        }

        // Show ticket types
        public IActionResult Tickets()
        {
            var ticketTypes = new List<object>
            {
                new { Type = "Single Ride", Price = 15, Description = "Valid for one trip between any two stations." },
                new { Type = "Daily Pass", Price = 40, Description = "Unlimited rides for 24 hours." },
                new { Type = "Weekly Pass", Price = 150, Description = "Unlimited rides for 7 days." },
                new { Type = "Monthly Pass", Price = 500, Description = "Unlimited rides for 30 days." }
            };

            return View(ticketTypes);
        }

        // GET: Book ticket page
        [HttpGet]
        public async Task<IActionResult> BookTicket(string? type = null, string? price = null, int? startStationId = null, int? endStationId = null)
        {
            var viewModel = new BookTicketViewModel
            {
                TicketType = type,
                Price = string.IsNullOrEmpty(price) ? 0 : decimal.Parse(price.Replace(" EGP", ""))
            };

            if (startStationId.HasValue) viewModel.FromStation = startStationId.ToString();
            if (endStationId.HasValue) viewModel.ToStation = endStationId.ToString();

            ViewBag.Stations = await _context.Stations.Where(s => s.Status == "Active").ToListAsync();
            return View(viewModel);
        }

        // POST: Book ticket
        [HttpPost]
        public async Task<IActionResult> BookTicket(BookTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _context.Stations.Where(s => s.Status == "Active").ToListAsync();
                return View(model);
            }

            // Parse station IDs
            if (!int.TryParse(model.FromStation, out int startStationId) ||
                !int.TryParse(model.ToStation, out int endStationId))
            {
                ModelState.AddModelError("", "Please select valid start and end stations.");
                ViewBag.Stations = await _context.Stations.Where(s => s.Status == "Active").ToListAsync();
                return View(model);
            }

            // Find or create trip
            var trip = await _context.Trips
                .Include(t => t.StartStation)
                .Include(t => t.EndStation)
                .FirstOrDefaultAsync(t => t.StartStationID == startStationId && t.EndStationID == endStationId);

            if (trip == null)
            {
                var startStation = await _context.Stations.FindAsync(startStationId);
                var endStation = await _context.Stations.FindAsync(endStationId);

                trip = new Trip
                {
                    StartStationID = startStationId,
                    EndStationID = endStationId,
                    StartStation = startStation!,
                    EndStation = endStation!,
                    Distance = 10.0m, // You can calculate real distance here
                    Type = model.TicketType ?? "Regular"
                };

                // Calculate price automatically
                trip.CalculatePrice();

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
            }

            // Create ticket
            var ticket = new Ticket
            {
                TripID = trip.TripID,
                PurchaseDate = DateTime.Now
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            TempData["TicketId"] = ticket.TicketID;
            return RedirectToAction(nameof(TicketConfirmation), new { id = ticket.TicketID });
        }

        // Ticket confirmation page
        public async Task<IActionResult> TicketConfirmation(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.StartStation)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.EndStation)
                .FirstOrDefaultAsync(t => t.TicketID == id);

            if (ticket == null) return NotFound();

            return View(ticket);
        }

        // Tickets management page
        public async Task<IActionResult> TicketsManagement()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.StartStation)
                .Include(t => t.Trip)
                    .ThenInclude(tr => tr.EndStation)
                .OrderByDescending(t => t.PurchaseDate)
                .ToListAsync();

            return View(tickets);
        }
    }
}
