using MAlex.Models.viewModels;
using MetroApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MAlex.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AppDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            AppDbContext context,
            IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
        }

        // ================================
        // REGISTER
        // ================================


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel userViewModel)
        {
            if (!ModelState.IsValid)
                return View(userViewModel);


            User user = new User
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email
            };

            // Create User
            IdentityResult result = await userManager.CreateAsync(user, userViewModel.Password);

            if (result.Succeeded)
            {
                // Add Visitor role
                if (!await userManager.IsInRoleAsync(user, "Visitor"))
                {
                    await userManager.AddToRoleAsync(user, "Visitor");
                }

                // Automatically confirm email
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                return View("RegistrationSuccessful");
            }



            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Register", userViewModel);
        }





        [HttpGet]
        public IActionResult ConfirmCode()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> ConfirmCode(ConfirmCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }

            var user = await userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return View(model);

            }

            if (user.EmailConfirmationCode == model.Code && user.EmailConfirmationCodeExpiry > DateTime.UtcNow)
            {

                user.EmailConfirmed = true;
                user.EmailConfirmationCode = null;
                user.EmailConfirmationCodeExpiry = null;
                await
                    userManager.UpdateAsync(user);
                return View("ConfirmEmail");


            }
            ModelState.AddModelError("", "Invalid or Ex code");
            return View(model);

        }






        // ================================
        // CONFIRM EMAIL
        // ================================
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                return RedirectToAction("Index", "Home");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
                return View("ConfirmEmail");

            return View("Error");
        }


        // ================================
        // LOGIN
        // ================================
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Login", model);

            var user = await userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View("Login", model);
            }

            //if (!await userManager.IsEmailConfirmedAsync(user))
            //{
            //    ModelState.AddModelError("", "Please confirm your email first.");
            //    return View("Login", model);
            //}


            var result = await signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is locked.");
                return View("Login", model);
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View("Login", model);
        }

        // ================================
        // LOGOUT
        // ================================
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        // ================================
        // PROFILE
        // ================================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {

            var user = await userManager.GetUserAsync(User);
            var roles = await userManager.GetRolesAsync(user);

            ViewBag.Roles = roles;



            return View("Profile", user);
        }

        [HttpGet]
        public IActionResult UpdateProfile()
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
                        return RedirectToAction("Index", "Home");

                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View("Profile", profileViewModel);
        }




        [HttpGet]
        public async Task<IActionResult> ForgetPassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existEmail = await userManager.FindByEmailAsync(model.Email);

            if (existEmail == null)
                return RedirectToAction("ForgetPasswordConfirmation");



            var code = await userManager.GenerateTwoFactorTokenAsync(existEmail, TokenOptions.DefaultEmailProvider);



            await emailSender.SendEmailAsync(
               existEmail.Email,
                "Reset your Password",
               $"code {code}"
               );



            // later 


            return View("ResetpasswordWithCode", new ResetPassViewModel { Email = existEmail.Email });


        }

        [HttpGet]
        public async Task<IActionResult> ResetpasswordWithCode(string email)
        {

            return View(new ResetPassViewModel { Email = email });
        }


        [HttpPost]
        public async Task<IActionResult> ResetpasswordWithCode(ResetPassViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);


            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return RedirectToAction("ResetPasswordConformation");


            var isCodeValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, model.Code);

            if (!isCodeValid)
            {
                ModelState.AddModelError("", "Invalid or Expired Code ");
                return View(model);
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var result = await
                userManager.ResetPasswordAsync(
                        user,
                        resetToken,
                        model.NewPassword
                    );

            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConformation");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);



        }




        public static async Task CreateAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "Ahmed1",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    // Assign Admin role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

        }
    }
}
