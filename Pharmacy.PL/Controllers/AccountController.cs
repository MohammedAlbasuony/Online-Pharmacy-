using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NuGet.Protocol.Plugins;
using Pharmacy.DAL.DB;
using Pharmacy.DAL.Entity;
using Pharmacy.PL.ViewModels;
using System.Data;
using System.Text;

namespace Pharmacy.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext _db;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _db = db;
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
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest("User already exists");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int? userTypeId = null;
            string role = model.UserType.ToString();

            // Start transaction if needed
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                // Create ApplicationUser instance first
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.Name,
                    UserType = model.UserType
                };

                IdentityResult userCreateResult = await userManager.CreateAsync(user, model.Password);
                if (!userCreateResult.Succeeded)
                {
                    foreach (var error in userCreateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Initialize userTypeId and add appropriate user-related data
                switch (model.UserType)
                {
                    case EnUserType.Patient:
                        var patient = new Patient
                        {
                            Name = model.Name,
                            MedicalHistory = "No medical history",
                            ApplicationUserId = user.Id // Assign the UserId
                        };
                        _db.Patients.Add(patient);
                        await _db.SaveChangesAsync();
                        userTypeId = patient.PatientID;
                        break;

                    case EnUserType.Doctor:
                        var doctor = new Doctor
                        {
                            Name = model.Name,
                        };
                        _db.Doctors.Add(doctor);
                        await _db.SaveChangesAsync();
                        userTypeId = doctor.DoctorID;
                        break;

                    case EnUserType.Pharmacist:
                        var pharmacist = new Pharmacist
                        {
                            Name = model.Name
                        };
                        _db.Pharmacists.Add(pharmacist);
                        await _db.SaveChangesAsync();
                        userTypeId = pharmacist.PharmacistID;
                        break;

                    default:
                        ModelState.AddModelError("", "Invalid user type");
                        return View(model);
                }

                // Now assign the userTypeId to the user
                user.UserTypeID = userTypeId.Value;

                // Add the user to the appropriate role
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRoleAsync(user, role);

                // Commit the transaction
                await transaction.CommitAsync();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while creating your account.");
                return View(model);
            }
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
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return View(model);
            }


            var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordValid)
            {

                ModelState.AddModelError(string.Empty, "Invalid credentials.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");

              
        }
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(VerifyEmailVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
            }
        }

        [HttpGet]
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }

            return View(new ChangePasswordVm { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }

            var result = await userManager.RemovePasswordAsync(user);
            if (result.Succeeded)
            {
                result = await userManager.AddPasswordAsync(user, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
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
