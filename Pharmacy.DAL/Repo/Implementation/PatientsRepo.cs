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

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var user = await _DBcontext.Users.FindAsync(id);
                if (user != null)
                {
                    _DBcontext.Users.Remove(user); // Hard delete
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

        public async Task<bool> UpdateAsync(Patient patient)
        {
            try
            {
                var result = await _DBcontext.Patients.Where(d => d.ApplicationUserId == patient.ApplicationUserId).FirstOrDefaultAsync();


                if (result != null)
                {
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
