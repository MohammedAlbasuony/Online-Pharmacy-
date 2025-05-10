using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class ManualReview
    {
        public int ReviewID { get; set; }
        public int PatientID { get; set; }
        public int PrescriptionID { get; set; }

        public Prescription Prescription { get; set; }
        public Patient Patient { get; set; }
    }
}
