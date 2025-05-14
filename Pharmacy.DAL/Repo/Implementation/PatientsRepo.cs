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
    public class PatientsRepo : IPatientsRepo
    {
        private readonly ApplicationDbContext _DBcontext;

        public PatientsRepo(ApplicationDbContext DBcontext)
        {
            _DBcontext = DBcontext;
        }

        public async Task<bool> AddAsync(Patient patient)
        {
            try
            {
                await _DBcontext.Patients.AddAsync(patient);
                await _DBcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var patient = await _DBcontext.Patients.FindAsync(id); // Corrected the double dot and ensured proper usage of FindAsync.  
                if (patient != null)
                {
                    _DBcontext.Patients.Remove(patient); // Corrected the DbSet reference to Patients instead of Users.  
                    await _DBcontext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting patient: {ex.Message}");
                return false;
            }
        }


        public async Task<List<Patient>> GetAllAsync()
        {
            try
            {
                return await _DBcontext.Patients
            .Include(d => d.ApplicationUser)
            .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return new List<Patient>();
            }
        }

        public async Task<Patient> GetByIdAsync(string id)
        {
            try
            {
                return await _DBcontext.Patients
             .Include(d => d.ApplicationUser)
             .FirstOrDefaultAsync(d => d.ApplicationUserId == id );
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return new Patient();
            }
        }
        public async Task<Patient> GetByPatientIdAsync(int patientId)
        {
            try
            {
                // Fetch the patient by PatientID and include the ApplicationUser (related data)
                return await _DBcontext.Patients
                    .Include(d => d.ApplicationUser) // Include related ApplicationUser
                    .FirstOrDefaultAsync(d => d.PatientID == patientId); // Use PatientID instead of ApplicationUserId
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return null; // Or you can return a new Patient() if preferred
            }
        }


        public async Task<bool> UpdateAsync(Patient patient)
        {
            try
            {
                var result = await _DBcontext.Patients.Where(d => d.PatientID == patient.PatientID).FirstOrDefaultAsync();


                if (result != null)
                {
                    result.Name = patient.Name;
                    result.MedicalHistory = patient.MedicalHistory;
                    await _DBcontext.SaveChangesAsync();
                    return true;
                }
                return false; // Patient not found
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                return false;
            }
        }
    }
}
