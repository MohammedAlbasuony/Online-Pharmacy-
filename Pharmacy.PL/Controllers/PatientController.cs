using Hospital_Mangment_System_BLL.View_model.patientVM;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.BLL.Service.Abstraction;

namespace Pharmacy.PL.Controllers
{
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllPatient()
        {
            var result = await _patientService.GetAllAsync();
            return View(result);
        }

        public async Task<IActionResult> GetPatientById(string id)
        {
            var result = await _patientService.GetByIdAsync(id);
            return View(result);
        }

        [HttpGet]
        public IActionResult AddPatient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(CreatePatientVM patientVM)
        {
            if (ModelState.IsValid)
            {
                await _patientService.AddAsync(patientVM);
                return RedirectToAction("GetAllPatient");
            }
            return View(patientVM);
        }

        public async Task<IActionResult> DeletePatient(string id)
        {
            await _patientService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePatient(string id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatient(UpdatePatientVM patientVM)
        {
            if (ModelState.IsValid)
            {
                await _patientService.UpdateAsync(patientVM);
                return RedirectToAction("GetAllPatient");
            }
            return View(patientVM);
        }

    }
}
