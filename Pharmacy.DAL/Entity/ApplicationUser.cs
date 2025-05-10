using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class ApplicationUser: IdentityUser
    {

        public string? FullName { get; set; }
        public string? LicenseNumber { get; set; } // For doctors and pharmacists
        public string ?Specialization { get; set; } // For doctors
        // Navigation properties
        public virtual ICollection<Prescription>? Prescriptions { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public ICollection<Payment>? Payments { get; set; }

    }
}
