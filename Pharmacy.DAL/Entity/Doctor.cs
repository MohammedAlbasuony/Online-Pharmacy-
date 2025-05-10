using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string Password { get; set; }

        public ICollection<Prescription> Prescriptions { get; set; }
    }
}
