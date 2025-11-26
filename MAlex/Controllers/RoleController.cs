using MAlex.Models.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MAlex.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;

        }
        public IActionResult AddRole()
        {
            return View("AddRole");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(RoleViewModel roleViewModel) {

            if (ModelState.IsValid) {
                //save db

                var visitorRole = await roleManager.FindByNameAsync("Visitor");
                visitorRole = new IdentityRole("Visitor");
                await roleManager.CreateAsync(visitorRole);



                IdentityRole role = new IdentityRole();
                role.Name = roleViewModel.RoleName;

              IdentityResult result = await roleManager.CreateAsync(role);

                if (result.Succeeded) {
                    //cookie
                    ViewBag.success = true;
                    return View("AddRole", roleViewModel);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View("AddRole", roleViewModel);

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Roles()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

    }
}
