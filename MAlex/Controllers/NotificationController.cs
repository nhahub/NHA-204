using MAlex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MAlex.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var notifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(notifications);
        }


        public async Task<IActionResult> Admin()
        {
            var recentNotifications = await _context.Notifications
                .OrderByDescending(n => n.CreatedAt)
                .Take(20)
                .ToListAsync();

            return View(recentNotifications);
        }


        [HttpPost]
        
        public async Task<IActionResult> Create(Notification notification)
        {
            if (ModelState.IsValid)
            {
                notification.CreatedAt = DateTime.Now;
                notification.IsActive = true;

                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Notification created successfully!";
                return RedirectToAction(nameof(Admin));
            }

            TempData["ErrorMessage"] = "Failed to create notification.";
            return RedirectToAction(nameof(Admin));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var noti = await _context.Notifications.FindAsync(id);

            if (noti == null)
            {
                TempData["ErrorMessage"] = "Notification not found!";
                return RedirectToAction(nameof(Admin));
            }

            _context.Notifications.Remove(noti);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Notification deleted.";
            return RedirectToAction(nameof(Admin));
        }
    }
}