using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pharmacy.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL.DB;
using Microsoft.AspNetCore.Identity;
using Pharmacy.BLL.ViewModels;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;

    }
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
    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var userId = _userManager.GetUserId(User);
        var cart = GetCart();
        var userCart = cart.Where(c => c.UserId == userId).ToList();

        if (!userCart.Any())
            return RedirectToAction("Index", "Cart");  // Redirect to the Cart page if no items

        return View(userCart);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessCheckout()
    {
        var userId = _userManager.GetUserId(User);
        var cart = GetCart();
        var userCart = cart.Where(c => c.UserId == userId).ToList();

        if (!userCart.Any())
            return RedirectToAction("Index", "Cart");  // Ensure that there's data to process

        // Assuming you're getting the patient by the UserId
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.ApplicationUserId == userId);
        if (patient == null)
            return BadRequest("Patient not found.");

        // Check if all cart items have MedicationName set and are valid
        foreach (var item in userCart)
        {
            if (string.IsNullOrEmpty(item.MedicationName))
            {
                ModelState.AddModelError("", "One or more cart items are missing medication name.");
                return View("Checkout", userCart);  // Return to the checkout page with the error
            }
        }

        // Create the order from the user's cart
        var order = new Order
        {
            PatientID = patient.PatientID,
            OrderDate = DateTime.Now,
            Status = "Pending",
            TotalPrice = (double)userCart.Sum(c => c.UnitPrice * c.Quantity),
            OrderItems = userCart.Select(c => new OrderItem
            {
                MedicationId = c.MedicationId,
                MedicationName = c.MedicationName,  // Ensure MedicationName is populated
                Quantity = c.Quantity,
                UnitPrice = c.UnitPrice
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        // Create payment entry
        var payment = new Payment
        {
            OrderID = order.OrderID,
            PatientID = patient.PatientID,
            Amount = order.TotalPrice,
            PaymentMethod = "Cash",  // This could be extended to handle user input for payment method
            PaymentStatus = "Unpaid"
        };

        _context.Payments.Add(payment);

        // Remove items from cart after successful checkout
        cart.RemoveAll(c => c.UserId == userId);
        SaveCart(cart);

        await _context.SaveChangesAsync();

        TempData["Success"] = "Order placed successfully!";
        return RedirectToAction("OrderConfirmation", "Cart");  // Redirect to order list or orders page
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirmation()
    {
        var userId = _userManager.GetUserId(User);

        // Get the patient associated with this user
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

        if (patient == null)
            return BadRequest("Patient not found.");

        // Get the most recent order for the patient
        var order = await _context.Orders
            .Where(o => o.PatientID == patient.PatientID)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();

        if (order == null)
            return RedirectToAction("Index", "Cart");

        // Get related order items
        var orderItems = await _context.OrderItems
            .Where(oi => oi.OrderID == order.OrderID)
            .ToListAsync();

        var orderViewModel = new OrderViewModel
        {
            Order = order,
            OrderItems = orderItems
        };

        return View(orderViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmAndPay(int orderId)
    {
        var userId = _userManager.GetUserId(User);

        // Get the patient associated with this user
        var patient = await _context.Patients
            .FirstOrDefaultAsync(p => p.ApplicationUserId == userId);

        if (patient == null)
            return BadRequest("Patient not found.");

        // Get the most recent order for the patient
        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.PatientID == patient.PatientID && o.OrderID == orderId);

        if (order == null)
            return RedirectToAction("Index", "Cart");

        // Update the order status to "Paid"
        order.Status = "Paid";
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();

        TempData["Success"] = "Payment successful. Your order is now paid!";
        return RedirectToAction("OrderConfirmation", new { orderId = order.OrderID });
    }
    // Increase the quantity of the item in the cart
    public IActionResult IncreaseQuantity(int medicationId)
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var existing = cart.FirstOrDefault(x => x.MedicationId == medicationId && x.UserId == userId);
        if (existing != null && existing.Quantity < existing.MaxQuantity)
        {
            existing.Quantity++;  // Increase the quantity
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // Decrease the quantity of the item in the cart
    public IActionResult DecreaseQuantity(int medicationId)
    {
        var cart = GetCart();
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        var existing = cart.FirstOrDefault(x => x.MedicationId == medicationId && x.UserId == userId);
        if (existing != null && existing.Quantity > 1)
        {
            existing.Quantity--;  // Decrease the quantity
        }
        else if (existing != null && existing.Quantity == 1)
        {
            // If quantity is 1, remove the item from the cart
            cart.Remove(existing);
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }




}
