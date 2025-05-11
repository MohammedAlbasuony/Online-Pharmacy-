using Pharmacy.BLL.ViewModels.MedicineVM;
using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.Service.Abstraction
{
    public interface IMedicineService
    {
        Task<bool> AddAsync(AddMedicineVM medicine);
        Task<bool> DeleteAsync(int Id);
        Task<List<AddMedicineVM>> GetAllAsync();
        Task<UpdateMedicineVM> GetByIdAsync(int Id);
        Task<bool> UpdateAsync(UpdateMedicineVM medicine);
    }
}
