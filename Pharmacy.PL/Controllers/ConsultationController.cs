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

namespace Pharmacy.PL.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConsultationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                return View(model);

            // Ensure the model.PatientID (which is the selected PatientID) is valid
            var patient = await _context.Patients.FindAsync(model.PatientId); // Use PatientID to find the patient

            if (patient != null)
            {
                model.PatientId = patient.PatientID;
                model.PatientName = patient.Name;
                _context.Consultations.Add(model);
                await _context.SaveChangesAsync();
            }

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

            _context.Consultations.Update(model);
            await _context.SaveChangesAsync();
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
