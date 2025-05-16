using Microsoft.AspNetCore.Mvc;
using Pharmacy.BLL.Service.Abstraction;
using Pharmacy.BLL.ViewModels.MedicineVM;
using X.PagedList.Extensions;

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
            var medicines = await _medicineService.GetAllAsync();
            return View(medicines); // Pass the full list to the view
        }


        public async Task<IActionResult> GetMedicineById(int id)
        {
            var result = await _medicineService.GetByIdAsync(id);
            return View(result);
        }
        public async Task<IActionResult> MedicineDetails(int id)
        {
            var medicine = await _medicineService.GetByIdAsync(id); // Replace with your actual service
            if (medicine == null)
                return NotFound();

            return View(medicine);
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
                    TempData["Success"] = "Medicine added successfully!";
                    return View(Medicine);
                }
                ModelState.AddModelError("", "Unable to add Medicine. Please try again.");
            }

            return View(Medicine);
        }

        public async Task<IActionResult> DeleteMedicine(int id)
        {
            await _medicineService.DeleteAsync(id);
            TempData["DeleteSuccess"] = "Medicine deleted successfully!";
            return RedirectToAction(nameof(GetAllMedicine));
        }
        [HttpPost, ActionName("DeleteMedicine")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _medicineService.DeleteAsync(id);
            TempData["DeleteSuccess"] = "Medicine deleted successfully!";
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
                TempData["Success"] = "Medicine updated successfully!";
                return View(Medicine);
            }
            return View(Medicine);
        }
        [HttpPost]
        public async Task<IActionResult> Import(IFormFile excelFile)
        {
            if (excelFile != null && excelFile.Length > 0)
            {
                using var stream = excelFile.OpenReadStream();
                var result = await _medicineService.ImportFromExcelAsync(stream);
                if (result)
                    return RedirectToAction("GetAllMedicine"); // Or success view
            }

            ModelState.AddModelError("", "Invalid file. Please upload a valid Excel file.");
            return View("AddMedicine");
        }
    }
}

