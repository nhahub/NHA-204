using MAlex.Models;
using MAlex.Models.viewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAlex.Controllers
{
    public class TicketController : Controller
    {
        private readonly AppDbContext _context;

       
        public TicketController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Tickets()
        {
            var ticketTypes = await _context.TicketTypes
                .Select(tt => new TicketTypeViewModel
                {
                    Type = tt.Type,
                    Price = (int)tt.Price, 
                    Description = tt.Description
                })
                .ToListAsync();

            return View(ticketTypes);
        }




        // ============================
        //    My Tickets

        // ============================
        //--------------------------------------------------------------------------------it depend on User id in the cookie
        //public async Task<IActionResult> MyTrips()
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user == null) return Challenge();

        //    var trips = await _context.Trips
        //        .Where(trip => trip.Tickets
        //            .Any(ticket => ticket.UserTickets
        //                .Any(ut => ut.UserID == user.Id)))
        //        .Include(trip => trip.StartStation)
        //        .Include(trip => trip.EndStation)
        //        .Include(trip => trip.Tickets)
        //            .ThenInclude(t => t.UserTickets)
        //        .OrderByDescending(trip => trip.TripID)
        //        .ToListAsync();


        //    return View(trips);
        //}

        // ============================
        //     SHOW ALL TICKET TYPES
        // ============================


        // ============================
        //         BOOK TICKET (GET)
        // ============================
        [HttpGet]
        public async Task<IActionResult> BookTicket(string? type = null, int? price = null, int? startStationId = null, int? endStationId = null)
        {
            if (string.IsNullOrEmpty(type) || price == null)
            {
                return RedirectToAction("Tickets");
            }

            var ticket = await _context.TicketTypes
                .Where(tt => tt.Type == type)
                .Select(tt => new TicketTypeViewModel
                {
                    Type = tt.Type,
                    Price = (int)tt.Price,
                    Description = tt.Description
                })
                .FirstOrDefaultAsync();

            var viewModel = new BookTicketViewModel
            {
                TicketType = ticket?.Type,
                Price = ticket?.Price ?? 0,
                FromStation = startStationId?.ToString(),
                ToStation = endStationId?.ToString()
            };

            ViewBag.Stations = await _context.Stations
                .Where(s => s.Status == "Active")
                .ToListAsync();

            return View(viewModel);
        }


        // ============================
        //         BOOK TICKET (POST)
        // ============================
        [HttpPost]
        public async Task<IActionResult> BookTicket(BookTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Stations = await _context.Stations
                    .Where(s => s.Status == "Active")
                    .ToListAsync();
                return View(model);
            }

            if (!int.TryParse(model.FromStation, out int startStationId) ||
                !int.TryParse(model.ToStation, out int endStationId))
            {
                ModelState.AddModelError("", "Please select valid start and end stations.");
                ViewBag.Stations = await _context.Stations
                    .Where(s => s.Status == "Active")
                    .ToListAsync();
                return View(model);
            }

            // Check if trip exists
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
                    Distance = 10.0m,
                    Type = model.TicketType ?? "Regular"
                };

                if (model.Price <= 0)
                {
                    ModelState.AddModelError("", "Invalid ticket price.");
                    ViewBag.Stations = await _context.Stations
                        .Where(s => s.Status == "Active")
                        .ToListAsync();
                    return View(model);
                }

                trip.TotalPrice = model.Price;

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
            return RedirectToAction(nameof(TicketsManagement), new { id = ticket.TicketID });
        }


        // ============================
        //     TICKET CONFIRMATION
        // ============================
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

        // ============================
        //     ADMIN TICKET MANAGEMENT
        // ============================
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
