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
        public EnUserType UserType { get; set; } 
        public int UserTypeID { get; set; } // Foreign key for UserType
    }

    public enum EnUserType
    {
        Doctor =1,
        Pharmacist =2,
        Patient =3
    }
}
