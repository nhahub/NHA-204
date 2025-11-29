using MAlex.Models;
using MAlex.Models.viewModels;
using MAlex.ViewModels;
using MetroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MAlex.Controllers
{
    //[Authorize]
    public class UserSubscriptionsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        public UserSubscriptionsController(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: Show all available subscriptions for users
        public async Task<IActionResult> AvailableSubscriptions()
        {
            var list = await _db.Subscriptions.ToListAsync();
            return View(list);
        }

        // GET: /UserSubscriptions/My
        public async Task<IActionResult> My()
        {
            var userId = _userManager.GetUserId(User);
            var list = await _db.UserSubscriptions
                                .Include(us => us.Subscription)
                                .Where(us => us.UserID == userId)
                                .OrderByDescending(us => us.StartDate)
                                .ToListAsync();
            return View(list);
        }

        // GET: Purchase page
        public async Task<IActionResult> Purchase(int id)
        {
            var sub = await _db.Subscriptions.FindAsync(id);
            if (sub == null) return NotFound();
            
            var vm = new PurchaseSubscriptionViewModel
            {
                SubscriptionID = sub.SubscriptionID,
                Price = sub.Price,
                SubscriptionName = sub.Name,
                StartDate = DateTime.UtcNow.Date
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase(PurchaseSubscriptionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var subscription = await _db.Subscriptions.FindAsync(vm.SubscriptionID);
            if (subscription == null) return NotFound();

            var userId = _userManager.GetUserId(User);

            // ====== NEW FEATURE #1: Auto-cancel previous subscription ======
            var activeSub = await _db.UserSubscriptions
                .Where(us => us.UserID == userId && us.Status == SubscriptionStatus.Active)
                .OrderByDescending(us => us.EndDate)
                .FirstOrDefaultAsync();

            if (activeSub != null)
            {
                activeSub.Status = SubscriptionStatus.Cancelled;
            }

            // Set end date
            var endDate = vm.StartDate.AddDays(subscription.DurationDays);

            var userSub = new UserSubscription
            {
                UserID = userId,
                SubscriptionID = subscription.SubscriptionID,
                StartDate = vm.StartDate,
                EndDate = endDate,
                Status = SubscriptionStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _db.UserSubscriptions.Add(userSub);
            await _db.SaveChangesAsync();

            TempData["SuccessMessage"] = "Subscription purchased successfully!";
            return RedirectToAction(nameof(My));
        }

        // Allow user to cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            var us = await _db.UserSubscriptions.FirstOrDefaultAsync(x => x.UserSubscriptionID == id && x.UserID == userId);
            if (us == null) return NotFound();

            us.Status = SubscriptionStatus.Cancelled;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(My));
        }
    }
}