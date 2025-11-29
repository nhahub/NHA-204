using MAlex.Migrations;
using MAlex.Models;
using MAlex.Models.viewModels;
using MAlex.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;

namespace MAlex.Controllers.Admin
{
    //[Authorize(Roles = "Admin")]
    public class SubscriptionManagementController : Controller
    {
        private readonly AppDbContext _db;

        public SubscriptionManagementController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            // All user subscriptions including the user and subscription info
            var userSubs = await _db.UserSubscriptions
                                    .Include(us => us.User)
                                    .Include(us => us.Subscription)
                                    .ToListAsync();

            // All subscriptions
            var subscriptions = await _db.Subscriptions.ToListAsync();
         
            var vm = new SubscriptionManagementViewModel
            {
                UserSubscriptions = userSubs,
                Subscriptions = subscriptions
            };

            return View(vm);
        }


        public async Task<IActionResult> DetailsUserSubscription(int id)
        {
            var userSub = await _db.UserSubscriptions
                .Include(us => us.User)
                .Include(us => us.Subscription)
                .FirstOrDefaultAsync(us => us.UserSubscriptionID == id);

            if (userSub == null)
                return NotFound();

            return View(userSub);
        }

        //1. Create Subscription

        public IActionResult CreateSubscription()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSubscription(Subscrubtion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _db.Subscriptions.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Subscription created successfully!";
            return RedirectToAction(nameof(Index));
        }



        //2. Edit Subscription

        public async Task<IActionResult> EditSubscription(int id)
        {
            var sub = await _db.Subscriptions.FindAsync(id);
            if (sub == null) return NotFound();

            return View(sub);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubscription(Subscrubtion model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _db.Subscriptions.FindAsync(model.SubscriptionID);

            if (existing == null)
                return NotFound();

            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.Price = model.Price;
            existing.DurationDays = model.DurationDays;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Subscription updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        //3. Delete Subscription
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            var sub = await _db.Subscriptions.FindAsync(id);
            if (sub == null) return NotFound();

            return View(sub);
        }

        [HttpPost, ActionName("DeleteSubscription")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSubscriptionConfirmed(int id)
        {
            var sub = await _db.Subscriptions.FindAsync(id);

            if (sub != null)
            {
                _db.Subscriptions.Remove(sub);
                await _db.SaveChangesAsync();
            }

            TempData["Success"] = "Subscription deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


        // UserSubscription CRUD
        // GET: Create a new User Subscription
        public async Task<IActionResult> CreateUserSubscription()
        {
            // Load all users and subscriptions for dropdowns
            ViewBag.Users = await _db.Users.ToListAsync();
            ViewBag.Subscriptions = await _db.Subscriptions.ToListAsync();

            return View();
        }

        // POST: Create new User Subscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUserSubscription(UserSubscription userSub)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _db.Users.ToListAsync();
                ViewBag.Subscriptions = await _db.Subscriptions.ToListAsync();
                return View(userSub);
            }

            // Calculate EndDate based on selected subscription's DurationDays
            var subscription = await _db.Subscriptions.FindAsync(userSub.SubscriptionID);
            if (subscription != null)
            {
                userSub.StartDate = DateTime.UtcNow.Date;
                userSub.EndDate = userSub.StartDate.AddDays(subscription.DurationDays);
                userSub.Status = SubscriptionStatus.Active;
            }

            _db.UserSubscriptions.Add(userSub);
            await _db.SaveChangesAsync();

            // Redirect back to Management page so the table is refreshed
            return RedirectToAction("Index", "SubscriptionManagement");
        }


        //===========================
        public async Task<IActionResult> EditUserSubscription(int id)
        {
            var userSub = await _db.UserSubscriptions
                .Include(us => us.User)
                .Include(us => us.Subscription)
                .FirstOrDefaultAsync(us => us.UserSubscriptionID == id);

            if (userSub == null)
                return NotFound();

            // These two were missing → caused the NULL error
            ViewBag.Users = await _db.Users.ToListAsync();
            ViewBag.Subscriptions = await _db.Subscriptions.ToListAsync();

            return View(userSub);
        }


        [HttpPost]
        public async Task<IActionResult> EditUserSubscription(UserSubscription model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Users = await _db.Users.ToListAsync();
                ViewBag.Subscriptions = await _db.Subscriptions.ToListAsync();
                return View(model); // return same view with dropdowns
            }

            var existing = await _db.UserSubscriptions.FindAsync(model.UserSubscriptionID);

            if (existing == null)
                return NotFound();

            existing.UserID = model.UserID;
            existing.SubscriptionID = model.SubscriptionID;
            existing.StartDate = model.StartDate;
            existing.EndDate = model.EndDate;

            await _db.SaveChangesAsync();

            TempData["Success"] = "Subscription updated successfully!";
            return RedirectToAction("UserSubscriptionsList");
        }


        public async Task<IActionResult> DeleteUserSubscription(int id)
        {
            var userSub = await _db.UserSubscriptions
                                   .Include(us => us.User)
                                   .Include(us => us.Subscription)
                                   .FirstOrDefaultAsync(us => us.UserSubscriptionID == id);
            if (userSub == null) return NotFound();
            return View(userSub);
        }

        [HttpPost, ActionName("DeleteUserSubscription")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserSubscriptionConfirmed(int id)
        {
            var userSub = await _db.UserSubscriptions.FindAsync(id);
            if (userSub != null)
            {
                _db.UserSubscriptions.Remove(userSub);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Subscription CRUD can reuse your existing SubscriptionController methods
        // Or include them here similarly
    }
}
