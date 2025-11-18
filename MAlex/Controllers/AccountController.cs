using MAlex.Models.viewModels;
using MetroApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MAlex.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AppDbContext context; 
      
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager , AppDbContext context )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context; 
          
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userViewModel)
        {

            if (ModelState.IsValid)
            {
                //Map
                User user = new User();
                user.UserName = userViewModel.Username;
                user.PasswordHash = userViewModel.Password;


                //save db
                IdentityResult result = await userManager.CreateAsync(user, userViewModel.Password);


                if (result.Succeeded)
                {
                    //here i want to assign role to user
                    if (!await userManager.IsInRoleAsync(user, "Visitor"))
                    {
                        await userManager.AddToRoleAsync(user, "Visitor");
                    }

                    //cookie 
                    await signInManager.SignInAsync(user, false);
                    Claim Idclaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    string Id = Idclaim.Value;
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

             

            }
            return View("Register", userViewModel);
        }

        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");

        }


        [HttpPost]

        public async Task<IActionResult> SaveLogin(LoginUserViewModel UserViewMode)
        {
            if (ModelState.IsValid) {
                // check if user exsist
                User AppUser = await userManager.FindByNameAsync(UserViewMode.Username);
                if (AppUser != null) {
                    bool Found = await userManager.CheckPasswordAsync(AppUser, UserViewMode.Password);
                    if (Found) {
                        //create cookie
                        await signInManager.SignInAsync(AppUser, UserViewMode.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid username or password");

            }
            return View("Login", UserViewMode);

        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            return View("Profile", user);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            
            return View("UpdateProfile");
        }


        [HttpPost]

        public async Task<IActionResult> UpdateProfile(ProfileUserViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    user.UserName = profileViewModel.Username;
                    user.Email = profileViewModel.Email;
                    user.PhoneNumber = profileViewModel.PhoneNumber;
                    IdentityResult result = await userManager.UpdateAsync(user);


                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index", "Home");
                    }


                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View("Profile", profileViewModel);


        }

        public async Task<IActionResult> CreateAdmin()
        {
            
            var existingAdmin = await userManager.FindByEmailAsync("admin2@example.com");
            if (existingAdmin != null)
            {
                return Content("Admin already exists");
            }

          
            var adminUser = new User
            {
                UserName = "admin2",
                Email = "admin2@example.com"
            };

            var result = await userManager.CreateAsync(adminUser, "AdminPass123!"); 
            if (result.Succeeded)
            {
              
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }

                return Content("New admin created successfully!");
            }

            
            return Content(string.Join(", ", result.Errors.Select(e => e.Description)));
        }




    }
}
