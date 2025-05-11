using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.BLL.Contract;
using System.Security.Claims;

namespace Pharmacy.PL.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMedicationService _medicationService;
        private readonly IPrescriptionService _prescriptionService;

        public CartController(ICartService cartService, IMedicationService medicationService, IPrescriptionService prescriptionService)
        {
            _cartService = cartService;
            _medicationService = medicationService;
            _prescriptionService = prescriptionService;
        }

    }
}
