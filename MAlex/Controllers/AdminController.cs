using MetroApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MAlex.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AdminController(UserManager<User> userManager) { 
        
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult UserManagement()
        {
          List<User> users=  _userManager.Users.ToList();
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> LockUser(string Id) {

            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            return RedirectToAction("UserManagement"); 

        }

        [HttpPost]
        public async Task<IActionResult> UnlockUser(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            await _userManager.SetLockoutEndDateAsync(user, null); 

            return RedirectToAction("UserManagement");

        }
    }
}
