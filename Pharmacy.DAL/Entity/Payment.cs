using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.Entity
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public int PatientID { get; set; }
        public double Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public Order Order { get; set; }
        public Patient Patient { get; set; }
    }
}
