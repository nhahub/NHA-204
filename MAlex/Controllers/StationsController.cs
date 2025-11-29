using Microsoft.AspNetCore.Mvc;
using MAlex.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace MAlex.Controllers
{
    public class StationsController : Controller
    {
        private readonly AppDbContext _context;

        public StationsController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var stations = _context.Stations.ToList();
            return View(stations);
        }


        public IActionResult Manage()
        {
            var stations = _context.Stations.ToList();
            return View(stations);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Station station)
        {
            if (ModelState.IsValid)
            {
                _context.Stations.Add(station);
                _context.SaveChanges();
                return RedirectToAction("Manage");
            }

            var stations = _context.Stations.ToList();
            return View("Manage", stations);
        }


        public IActionResult ToggleStatus(int id)
        {
            var station = _context.Stations.Find(id);
            if (station != null)
            {
                station.Status = (station.Status == "Active") ? "Inactive" : "Active";
                _context.SaveChanges();
            }
            return RedirectToAction("Manage");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            var station = _context.Stations.Find(id);

            if (station == null)
            {

                return NotFound();
            }


            _context.Stations.Remove(station);


            _context.SaveChanges();


            return RedirectToAction(nameof(Manage));
        }
    }
}