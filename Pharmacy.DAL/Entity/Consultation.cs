using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public enum ConsultationType
    {
        VideoCall,
        InPerson,
        PhoneCall
    }

    public enum ConsultationStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
    public class Consultation
    {
        public int Id { get; set; }
        public int PatientId { get; set; }  // Add this line to associate the consultation with a patient
        public string? PatientName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public ConsultationType Type { get; set; }
        public ConsultationStatus Status { get; set; }
        // Foreign Key to Patient
        public Patient? Patient { get; set; }  // Navigation property to the Patient entity
    }

}
