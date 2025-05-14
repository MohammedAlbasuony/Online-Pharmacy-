using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.ViewModels
{
    public class StatsViewModel
    {
        public int DoctorCount { get; set; }
        public int PharmacistCount { get; set; }
        public int PatientCount { get; set; }
    }

}
