using Microsoft.AspNetCore.Mvc;

namespace Pharmacy.PL.Controllers
{
    public class ConsultationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
