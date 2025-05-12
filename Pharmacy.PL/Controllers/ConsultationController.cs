using Microsoft.AspNetCore.Mvc;
using Pharmacy.DAL.Entity;

namespace Pharmacy.PL.Controllers
{
    public class ConsultationController : Controller
    {
        private static List<Consultation> _consultations = new List<Consultation>
        {
            new Consultation {
                Id = 1,
                PatientName = "John Doe",
                Date = new DateTime(2025, 4, 5),
                Time = new TimeSpan(10, 30, 0),
                Type = ConsultationType.VideoCall,
                Status = ConsultationStatus.Scheduled
            },
            new Consultation {
                Id = 2,
                PatientName = "Sarah Williams",
                Date = new DateTime(2025, 4, 6),
                Time = new TimeSpan(14, 0, 0),
                Type = ConsultationType.InPerson,
                Status = ConsultationStatus.Scheduled
            },
            new Consultation {
                Id = 3,
                PatientName = "Emily Smith",
                Date = new DateTime(2025, 3, 28),
                Time = new TimeSpan(9, 0, 0),
                Type = ConsultationType.VideoCall,
                Status = ConsultationStatus.Completed
            }
        };

        // GET: /consultation
        public IActionResult Index()
        {
            return View(_consultations);
        }

        // GET: /consultation/upcoming
        public IActionResult Upcoming()
        {
            var upcoming = _consultations.Where(c => c.Status == ConsultationStatus.Scheduled).ToList();
            return View("Index", upcoming);
        }

        // GET: /consultation/past
        public IActionResult Past()
        {
            var past = _consultations.Where(c => c.Status != ConsultationStatus.Scheduled).ToList();
            return View("Index", past);
        }

        // GET: /consultation/details/1
        public IActionResult Details(int id)
        {
            var consult = _consultations.FirstOrDefault(c => c.Id == id);
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
        public IActionResult Create(Consultation model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Id = _consultations.Count > 0 ? _consultations.Max(c => c.Id) + 1 : 1;
            _consultations.Add(model);
            return RedirectToAction("Index");
        }

        // GET: /consultation/edit/1
        public IActionResult Edit(int id)
        {
            var consult = _consultations.FirstOrDefault(c => c.Id == id);
            if (consult == null)
                return NotFound();

            return View(consult);
        }

        // POST: /consultation/edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Consultation model)
        {
            var index = _consultations.FindIndex(c => c.Id == id);
            if (index == -1)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            _consultations[index] = model;
            return RedirectToAction("Index");
        }

        // GET: /consultation/delete/1
        public IActionResult Delete(int id)
        {
            var consult = _consultations.FirstOrDefault(c => c.Id == id);
            if (consult == null)
                return NotFound();

            return View(consult);
        }

        // POST: /consultation/delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var consult = _consultations.FirstOrDefault(c => c.Id == id);
            if (consult != null)
                _consultations.Remove(consult);

            return RedirectToAction("Index");
        }
    }
}

