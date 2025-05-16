using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.BLL.ViewModels;
using Pharmacy.DAL.DB;


namespace Pharmacy.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Replace 'object' with your actual DbContext type  

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var doctorCount = _context.Doctors.Count();
            var pharmacistCount = _context.Pharmacists.Count();
            var patientCount = _context.Patients.Count();

            var stats = new StatsViewModel
            {
                DoctorCount = doctorCount,
                PharmacistCount = pharmacistCount,
                PatientCount = patientCount
            };

            return View(stats);
        }
        public IActionResult PowerBI()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
