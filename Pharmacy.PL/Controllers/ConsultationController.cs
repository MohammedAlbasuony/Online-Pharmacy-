using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL;
using Pharmacy.DAL.DB;
using Pharmacy.DAL.Entity;
using X.PagedList;
using X.PagedList.Extensions;

namespace Pharmacy.PL.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConsultationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /consultation
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 2;
            var consultations = await _context.Consultations.OrderBy(c => c.Date).ToListAsync();
            var pagedConsultations = consultations.ToPagedList(page, pageSize);
            return View(pagedConsultations);
        }


        public async Task<IActionResult> Upcoming(int page = 1)
        {
            int pageSize = 2;

            var upcomingList = await _context.Consultations
                .Where(c => c.Status == ConsultationStatus.Scheduled)
                .OrderBy(c => c.Date)
                .ToListAsync();

            var pagedUpcoming = upcomingList.ToPagedList(page, pageSize);
            return View("Index", pagedUpcoming);
        }
        public async Task<IActionResult> Past(int page = 1)
        {
            int pageSize = 2;

            var pastList = await _context.Consultations
                .Where(c => c.Status != ConsultationStatus.Scheduled)
                .OrderByDescending(c => c.Date)
                .ToListAsync();

            var pagedPast = pastList.ToPagedList(page, pageSize);
            return View("Index", pagedPast);
        }


        // GET: /consultation/details/1
        public async Task<IActionResult> Details(int id)
        {
            var consult = await _context.Consultations.FirstOrDefaultAsync(c => c.Id == id);
            if (consult == null)
                return NotFound();

            return View(consult);
        }

        // GET: /consultation/create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /consultation/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consultation model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Consultations.Add(model);
            await _context.SaveChangesAsync();
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
