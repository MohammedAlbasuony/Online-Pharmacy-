using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Pharmacy.DAL.DB;
using Pharmacy.DAL.Entity;
using Pharmacy.DAL.Repo.Abstraction;

namespace Pharmacy.DAL.Repo.Implementation
{
    public class MedicineRepo : IMedicineRepo
    {
        private readonly ApplicationDbContext _DBcontext;

        public MedicineRepo(ApplicationDbContext context)
        {
            _DBcontext = context;
        }

        //  add method
        public async Task<bool> AddAsync(Medicine medicine)
        {
            try
            {
                await _DBcontext.Medicines.AddAsync(medicine);
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding doctor: {ex.Message}");
                return false;
            }
        }

        //  delete method

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var user = await _DBcontext.Medicines.FindAsync(id);
                if (user != null)
                {
                    _DBcontext.Medicines.Remove(user);
                    await _DBcontext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }


        //  get all method
        public async Task<List<Medicine>> GetAllAsync()
        {
            try
            {
                return await _DBcontext.Medicines
            .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctors: {ex.Message}");
                return new List<Medicine>();
            }
        }

        //  get by ID method
        public async Task<Medicine> GetByIdAsync(int id)
        {
            try
            {
                return await _DBcontext.Medicines
            .FirstOrDefaultAsync(d => d.MedicineID == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching doctor by ID: {ex.Message}");
                return null;
            }
        }




        //  update method
        public async Task<bool> UpdateAsync(Medicine medicine)
        {
            try
            {
                var existingMedicine = await _DBcontext.Medicines
                    .Where(p => p.MedicineID == medicine.MedicineID)
                    .FirstOrDefaultAsync();

                if (existingMedicine == null)
                {
                    return false;
                }

                // Update the properties
                existingMedicine.Name = medicine.Name;
                existingMedicine.Category = medicine.Category;
                existingMedicine.Price = medicine.Price;
                existingMedicine.StockQuantity = medicine.StockQuantity;
                existingMedicine.Manufacturer = medicine.Manufacturer;
                existingMedicine.ExpiryDate = medicine.ExpiryDate;
                existingMedicine.RequiresPrescription = medicine.RequiresPrescription;
                existingMedicine.ImageUrl = medicine.ImageUrl;
                existingMedicine.Uses = medicine.Uses;
                existingMedicine.SideEffects = medicine.SideEffects;

                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> ImportMedicinesFromExcelAsync(Stream stream)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null || worksheet.Dimension == null)
                    throw new Exception("No worksheet found or worksheet is empty.");

                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Skip header row
                {
                    var name = worksheet.Cells[row, 1].Text.Trim();
                    var category = worksheet.Cells[row, 2].Text.Trim();
                    var uses = worksheet.Cells[row, 3].Text.Trim();
                    var sideEffects = worksheet.Cells[row, 4].Text.Trim();
                    var imageUrl = worksheet.Cells[row, 5].Text.Trim();
                    var manufacturer = worksheet.Cells[row, 6].Text.Trim();
                    var priceText = worksheet.Cells[row, 7].Text.Trim();
                    var quantityText = worksheet.Cells[row, 8].Text.Trim();
                    var expiryDateText = worksheet.Cells[row, 9].Text.Trim();
                    var prescriptionText = worksheet.Cells[row, 10].Text.Trim();

                    // Validate mandatory fields
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(category) ||
                        string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(quantityText))
                    {
                        Console.WriteLine($"Skipping row {row}: Missing mandatory fields.");
                        continue;
                    }

                    // Try parsing
                    bool priceParsed = double.TryParse(priceText, out double price);
                    bool quantityParsed = int.TryParse(quantityText, out int quantity);
                    bool expiryParsed = DateTime.TryParse(expiryDateText, out DateTime expiryDate);

                    // Handle custom "Yes"/"No" values for RequiresPrescription
                    bool requiresPrescription = false;
                    if (!string.IsNullOrWhiteSpace(prescriptionText))
                    {
                        // Check for "Yes" or "No"
                        requiresPrescription = prescriptionText.Equals("Yes", StringComparison.OrdinalIgnoreCase);
                    }

                    if (!priceParsed || !quantityParsed || !expiryParsed)
                    {
                        Console.WriteLine($"Skipping row {row}: Invalid data format.");
                        continue;
                    }

                    var medicine = new Medicine
                    {
                        Name = name,
                        Category = category,
                        Price = price,
                        StockQuantity = quantity,
                        Manufacturer = manufacturer,
                        ExpiryDate = expiryDate,
                        RequiresPrescription = requiresPrescription,
                        ImageUrl = imageUrl,
                        Uses = uses,
                        SideEffects = sideEffects
                    };

                    // Save to DB
                    await AddAsync(medicine);

                    Console.WriteLine($"Imported medicine '{name}' successfully.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error importing medicines: {ex.Message}");
                return false;
            }
        }



    }
}


