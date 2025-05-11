using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.BLL.Service.Abstraction;
using Pharmacy.BLL.Service.Implementation;
using Pharmacy.BLL.ViewModels.MedicineVM;

namespace Pharmacy.PL.Controllers
{
    public class MedicineController : Controller
    {
        private readonly IMedicineService _medicineService;

        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAllMedicine()
        {
            var result = await _medicineService.GetAllAsync();
            return View(result);
        }

        public async Task<IActionResult> GetMedicineById(int id)
        {
            var result = await _medicineService.GetByIdAsync(id);
            return View(result);
        }

        [HttpGet]
        public IActionResult AddMedicine()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddMedicine(AddMedicineVM Medicine)
        {
            if (ModelState.IsValid)
             {
                var result = await _medicineService.AddAsync(Medicine);
                if (result)
                {
                    return RedirectToAction("GetAllMedicine");
                }
                ModelState.AddModelError("", "Unable to add Medicine. Please try again.");
            }

            return View(Medicine);
        }

        public async Task<IActionResult> DeleteMedicine(int id)
        {
            await _medicineService.DeleteAsync(id);
            return RedirectToAction(nameof(GetAllMedicine));
        }
        [HttpPost, ActionName("DeleteMedicine")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicineService.DeleteAsync(id);
            return RedirectToAction(nameof(GetAllMedicine));
        }
        [HttpGet]
        public async Task<IActionResult> UpdateMedicine(int id)
        {
            var medicine = await _medicineService.GetByIdAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMedicine(UpdateMedicineVM Medicine)
        {
            if (ModelState.IsValid)
            {
                await _medicineService.UpdateAsync(Medicine);
                return RedirectToAction("GetAllMedicine");
            }
            return View(Medicine);
        }
    }
}

