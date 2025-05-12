using Pharmacy.BLL.ViewModels.PatientVm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.Service.Abstraction
{
    public interface IPatientService
    {
        Task<bool> AddAsync(CreatePatientVM patient);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(UpdatePatientVM patient);
        Task<GetPatientByIdVM> GetByIdAsync(string id);
        Task<List<GetAllPatientssVM>> GetAllAsync();
    }
}
