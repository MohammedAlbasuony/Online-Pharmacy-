using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.PL.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
