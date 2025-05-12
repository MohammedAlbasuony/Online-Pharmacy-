using Pharmacy.DAL.Entity;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Mangment_System_BLL.View_model.patientVM
{
    public class UpdatePatientVM
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string MedicalHistory { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<Prescription>? Prescriptions { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public string? ApplicationUserId { get; set; }
    }

}
