using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL.DB;
using Pharmacy.DAL.Entity;
using Pharmacy.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating doctor: {ex.Message}");
                return false;
            }
        }
    }
}

