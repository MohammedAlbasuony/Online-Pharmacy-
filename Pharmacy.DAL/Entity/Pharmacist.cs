using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Pharmacist
    {
        public int PharmacistID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string Password { get; set; }
    }
}
