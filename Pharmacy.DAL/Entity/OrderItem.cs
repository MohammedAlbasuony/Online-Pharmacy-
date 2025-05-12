using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int MedicationId { get; set; }
        public string MedicationName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsPrescriptionRequired { get; set; }

        public Order Order { get; set; }
        public Medicine Medicine { get; set; }
    }

}
