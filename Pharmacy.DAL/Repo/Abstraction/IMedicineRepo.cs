using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Repo.Abstraction
{
    public interface IMedicineRepo
    {
        Task<bool> AddAsync(Medicine medicine);
        Task<bool> DeleteAsync(int id);
        Task<List<Medicine>> GetAllAsync();
        Task<Medicine> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Medicine medicine);
        Task<bool> ImportMedicinesFromExcelAsync(Stream stream);

    }
}
