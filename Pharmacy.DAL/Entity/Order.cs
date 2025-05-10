using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Order
    {
        public int OrderID { get; set; }
        public int PatientID { get; set; }
        public int MedicineID { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public Patient Patient { get; set; }
        public Medicine Medicine { get; set; }
        public Payment Payment { get; set; }
    }
}
