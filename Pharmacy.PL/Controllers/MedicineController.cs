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
        public async Task<IActionResult> GetAllMedicine(string searchQuery, int page = 1)
        {
            int pageSize = 10;  // Define the page size
            var medicines = await _medicineService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                medicines = medicines.Where(m =>
                    m.Name.ToLower().Contains(searchQuery) ||
                    m.Category.ToLower().Contains(searchQuery) ||
                    m.Manufacturer.ToLower().Contains(searchQuery) ||
                    m.Uses.ToLower().Contains(searchQuery) ||
                    m.SideEffects.ToLower().Contains(searchQuery)).ToList();
            }

            ViewBag.CurrentFilter = searchQuery;
            var pagedMedicines = medicines.ToPagedList(page, pageSize); // Paginate the list

            return View(pagedMedicines); // Pass the paginated result to the view
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

