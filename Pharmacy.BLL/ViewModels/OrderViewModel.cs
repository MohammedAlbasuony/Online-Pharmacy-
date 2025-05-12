using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.BLL.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }

}
