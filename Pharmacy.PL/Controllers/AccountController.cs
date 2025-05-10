using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.DAL.Entity;
using Pharmacy.PL.ViewModels;

namespace Pharmacy.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.Name,
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var roleExist = await roleManager.RoleExistsAsync("User");

                if (!roleExist)
                {
                    var role = new IdentityRole("User");
                    await roleManager.CreateAsync(role);
                }

                await userManager.AddToRoleAsync(user, "User");

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



    }
}
