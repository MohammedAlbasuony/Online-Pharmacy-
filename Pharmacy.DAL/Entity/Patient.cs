using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Address { get; set; }
        public string MedicalHistory { get; set; }
        public string Password { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Prescription> Prescriptions { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
