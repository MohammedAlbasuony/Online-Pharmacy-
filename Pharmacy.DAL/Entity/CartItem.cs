using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class CartItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public int? PrescriptionId { get; set; }
        public DateTime DateAdded { get; set; }
        public int MaxQuantity { get; set; }
    }
}
