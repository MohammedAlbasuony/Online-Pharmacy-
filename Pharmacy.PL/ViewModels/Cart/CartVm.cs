using Pharmacy.DAL.Entity;

namespace Pharmacy.PL.ViewModels.Cart
{
    public class CartVm
    {
       public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string CouponCode { get; set; }
        public bool CouponApplied { get; set; }
    }
}
