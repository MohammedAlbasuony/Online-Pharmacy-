using Pharmacy.BLL.Service.Abstraction;
using Pharmacy.BLL.ViewModels.MedicineVM;
using Pharmacy.DAL.Entity;
using Pharmacy.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.Service.Implementation
{
    public class MedicineService : IMedicineService
    {
        private readonly IMedicineRepo _medicineRepo;

        public MedicineService(IMedicineRepo medicineRepo)
        {
            _medicineRepo = medicineRepo;
        }

        public async Task<bool> AddAsync(AddMedicineVM medicine)
        {
            if (medicine != null)
            {
                var Medicine = new Medicine
                {
                    MedicineID = medicine.MedicineID,
                    Name = medicine.Name,
                    Category = medicine.Category,
                    Price = medicine.Price,
                    StockQuantity = medicine.StockQuantity,
                    Manufacturer = medicine.Manufacturer,
                    ExpiryDate = medicine.ExpiryDate,
                    RequiresPrescription = medicine.RequiresPrescription,
                    ImageUrl = medicine.ImageUrl,
                    Uses = medicine.Uses,
                    SideEffects = medicine.SideEffects,

                };
                return await _medicineRepo.AddAsync(Medicine);
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id != 0)
            {
                return await _medicineRepo.DeleteAsync(id);
            }
            return false;
        }

        public async Task<List<AddMedicineVM>> GetAllAsync()
        {
            var medicines = await _medicineRepo.GetAllAsync();

            // Manual Mapping and order by CreatedAt descending
            var medicineViewModels = medicines
                .Select(medicines => new AddMedicineVM
                {
                    MedicineID = medicines.MedicineID,
                    Name = medicines.Name,
                    Category = medicines.Category,
                    Price = medicines.Price,
                    StockQuantity = medicines.StockQuantity,
                    Manufacturer = medicines.Manufacturer,
                    ExpiryDate = medicines.ExpiryDate,
                    RequiresPrescription = medicines.RequiresPrescription,
                    ImageUrl = medicines.ImageUrl,
                    Uses = medicines.Uses,
                    SideEffects = medicines.SideEffects,

                })
                .ToList();

            return medicineViewModels;
        }

        public async Task<UpdateMedicineVM> GetByIdAsync(int id)
        {
            if (id != 0)
            {
                var medicine = await _medicineRepo.GetByIdAsync(id);

                if (medicine != null)
                {
                    return new UpdateMedicineVM
                    {
                        MedicineID = medicine.MedicineID,
                        Name = medicine.Name,
                        Category = medicine.Category,
                        Price = medicine.Price,
                        StockQuantity = medicine.StockQuantity,
                        Manufacturer = medicine.Manufacturer,
                        ExpiryDate = medicine.ExpiryDate,
                        RequiresPrescription = medicine.RequiresPrescription,
                        ImageUrl = medicine.ImageUrl,
                        Uses = medicine.Uses,
                        SideEffects = medicine.SideEffects,

                    };
                }
            }
            return null;
        }

        public async Task<bool> UpdateAsync(UpdateMedicineVM medicine)
        {
            if (medicine != null && medicine.MedicineID != 0)
            {
                // Fetch the customer by ID
                var existingMedicine = await _medicineRepo.GetByIdAsync(medicine.MedicineID);

                if (existingMedicine != null)
                {
                    // Update properties
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


                    return await _medicineRepo.UpdateAsync(existingMedicine);
                }
            }
            return false;
        }
        public async Task<bool> ImportFromExcelAsync(Stream stream)
        {
            return await _medicineRepo.ImportMedicinesFromExcelAsync(stream);
        }
    }
}
