﻿using Hospital_Mangment_System_BLL.View_model.patientVM;

namespace Pharmacy.BLL.Service.Abstraction
{
    public interface IPatientService
    {
        Task<bool> AddAsync(CreatePatientVM patient);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateAsync(UpdatePatientVM patient);
        Task<GetPatientByIdVM> GetByIdAsync(string id);
        Task<List<GetAllPatientssVM>> GetAllAsync();
        Task<UpdatePatientVM> GetByPatientIdAsync(int id);
    }
}
