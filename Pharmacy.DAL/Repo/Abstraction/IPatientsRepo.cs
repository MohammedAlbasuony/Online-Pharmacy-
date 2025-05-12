using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Repo.Abstraction
{
    public interface IPatientsRepo
    {
        Task<bool> AddAsync(Patient patient);
        Task<bool> DeleteAsync(string id);
        Task<List<Patient>> GetAllAsync();
        Task<Patient> GetByIdAsync(string id);
        Task<bool> UpdateAsync(Patient patient);


    }
}
