using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy.DAL.Entity;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class CartController : Controller
{
    private const string CartSessionKey = "UserCart";

    private List<CartItem> GetCart()
    {
        var sessionData = HttpContext.Session.GetString(CartSessionKey);
        if (string.IsNullOrEmpty(sessionData))
            return new List<CartItem>();

        return JsonConvert.DeserializeObject<List<CartItem>>(sessionData);
    }

    private void SaveCart(List<CartItem> cart)
    {
        HttpContext.Session.SetString(CartSessionKey, JsonConvert.SerializeObject(cart));
    }

    public IActionResult Index()
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var userCart = cart.Where(c => c.UserId == userId).ToList();
        return View(userCart);
    }

    public IActionResult AddToCart(int medicationId, string name, string dosage, decimal price, int maxQty, string imageUrl, bool isRx)
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var existing = cart.FirstOrDefault(x => x.MedicationId == medicationId && x.UserId == userId);
        if (existing != null)
        {
            if (existing.Quantity < existing.MaxQuantity)
                existing.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                UserId = userId,
                MedicationId = medicationId,
                MedicationName = name,
                Dosage = dosage,
                UnitPrice = price,
                Quantity = 1,
                MaxQuantity = maxQty,
                ImageUrl = imageUrl,
                IsPrescriptionRequired = isRx,
                DateAdded = DateTime.Now
            });
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    public IActionResult RemoveFromCart(int medicationId)
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var item = cart.FirstOrDefault(x => x.MedicationId == medicationId && x.UserId == userId);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }

        return RedirectToAction("Index");
    }

    public IActionResult ClearCart()
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        cart.RemoveAll(x => x.UserId == userId);
        SaveCart(cart);

        return RedirectToAction("Index");
    }
}
