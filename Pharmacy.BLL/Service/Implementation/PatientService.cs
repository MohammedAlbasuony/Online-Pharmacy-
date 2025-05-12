using Pharmacy.BLL.Service.Abstraction;
using Pharmacy.BLL.ViewModels.PatientVm;
using Pharmacy.DAL.Entity;
using Pharmacy.DAL.Repo.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.Service.Implementation
{
    public class PatientService:IPatientService
    {
        private readonly IPatientsRepo _patientsRepo;
        private readonly IMapper _mapper;

        public PatientService(IPatientsRepo patientsRepo, IMapper mapper) //Dependsncy InJection 
        {
            _patientsRepo = patientsRepo;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreatePatientVM patientVm)
        {

            var patient = _mapper.Map<Patient>(patientVm);
            return await _patientsRepo.AddAsync(patient);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _patientsRepo.DeleteAsync(id);
        }

        public async Task<List<GetAllPatientssVM>> GetAllAsync()
        {
            var patients = await _patientsRepo.GetAllAsync();
            return _mapper.Map<List<GetAllPatientssVM>>(patients);
        }

        public async Task<GetPatientByIdVM> GetByIdAsync(string id)
        {
            var patient = await _patientsRepo.GetByIdAsync(id);
            return _mapper.Map<GetPatientByIdVM>(patient);
        }

        public async Task<bool> UpdateAsync(UpdatePatientVM patientVm)
        {

            var patient = _mapper.Map<Patient>(patientVm);
            return await _patientsRepo.UpdateAsync(patient);
        }
    }


}
