using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Pharmacy.DAL;
using Pharmacy.DAL.Entity;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.DAL.DB;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Pharmacy.BLL.Service.Abstraction;

namespace Pharmacy.PL.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly IPatientService _patientService;
        
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsultationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPatientService patientService)
        {
            _context = context;
            _userManager = userManager;
            _patientService = patientService;

        }

        // GET: /consultation
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 12;
            var user = await _userManager.GetUserAsync(User); // Get the logged-in user

            // If the user is a Doctor, show all consultations
            if (User.IsInRole("Doctor"))
            {
                var consultations = await _context.Consultations.OrderBy(c => c.Date).ToListAsync();
                var pagedConsultations = consultations.ToPagedList(page, pageSize);
                return View(pagedConsultations);
            }

            // If the user is a Patient, show only their consultations
            if (User.IsInRole("Patient"))
            {
                var consultations = await _context.Consultations
                    .Where(c => c.Patient.ApplicationUserId == user.Id) // Filter by Patient ID
                    .OrderBy(c => c.Date)
                    .ToListAsync();

                var pagedConsultations = consultations.ToPagedList(page, pageSize);
                return View(pagedConsultations);
            }

            return View(new List<Consultation>().ToPagedList(page, pageSize)); // Empty list if no role found
        }

        // GET: /consultation/details/1
        public async Task<IActionResult> Details(int id)
        {
            var consult = await _context.Consultations.FirstOrDefaultAsync(c => c.Id == id);
            if (consult == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User); // Get the logged-in user

            // Check if the logged-in user is allowed to view this consultation
            if (User.IsInRole("Patient") && consult.Patient.ApplicationUserId != user.Id)
            {
                return Forbid(); // Patient can only see their own consultations
            }

            return View(consult);
        }

        // GET: /consultation/create
        public async Task<IActionResult> Create()
        {
            var patients = await _context.Patients.ToListAsync();
            ViewBag.Patients = new SelectList(patients, "PatientID", "Name"); // Create a dropdown list of patients
            return View();
        }


        // POST: /consultation/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consultation model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the view with the model to display validation errors
                return View(model);
            }

            // Ensure the model.PatientId (which is the selected PatientId) is valid
            var patient = await _context.Patients.FindAsync(model.PatientId);

            // Check if the patient exists
            if (patient == null)
            {
                // Add a model error if the patient was not found
                ModelState.AddModelError("PatientId", "The selected patient could not be found.");

                // Return the view with the model so the user can correct their input
                return View(model);
            }

            // If patient exists, populate the model fields
            model.PatientId = patient.PatientID;
            model.PatientName = patient.Name;

            // Add the consultation to the context and save
            _context.Consultations.Add(model);
            await _context.SaveChangesAsync();

            // Redirect to the Index page after successful save
            return RedirectToAction("Index");
        }




        // GET: /consultation/edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var consult = await _context.Consultations.FirstOrDefaultAsync(c => c.Id == id);
            if (consult == null)
                return NotFound();

            return View(consult);
        }

        // POST: /consultation/edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Consultation model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);
            model.PatientId = model.Id;

            // Ensure that the PatientId exists in the Patients table
            var patient = await _patientService.GetByPatientIdAsync(model.PatientId);
            if (patient == null)
            {
                // If the patient doesn't exist, add an error to the ModelState and return the view
                ModelState.AddModelError("PatientId", "The selected patient does not exist.");
                return View(model);
            }

            // If everything is valid, update the consultation
            try
            {
                _context.Consultations.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                // Handle DbUpdateException if a foreign key constraint violation occurs
                if (dbEx.InnerException is SqlException sqlEx && sqlEx.Number == 547) // Foreign key violation
                {
                    ModelState.AddModelError("", "The operation violated a foreign key constraint. Please ensure all references are correct.");
                }
                else
                {
                    ModelState.AddModelError("", "An error occurred while saving the consultation. Please try again.");
                }
                return View(model);
            }

            return RedirectToAction("Index");
        }

        // POST: /consultation/delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var consult = await _context.Consultations.FirstOrDefaultAsync(c => c.Id == id);
            if (consult != null)
            {
                _context.Consultations.Remove(consult);
                await _context.SaveChangesAsync();
                TempData["DeleteSuccess"] = "Consultation has been deleted successfully.";
            }

            return RedirectToAction("Index");
        }
    }
}
