using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        public int MedicineID { get; set; }
        public string Dosage { get; set; }
        public DateTime IssueDate { get; set; }
        public string Status { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Medicine Medicine { get; set; }
    }
}
