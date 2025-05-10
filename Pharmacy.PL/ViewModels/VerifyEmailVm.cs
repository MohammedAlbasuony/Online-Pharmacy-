using System.ComponentModel.DataAnnotations;

namespace Pharmacy.PL.ViewModels
{
    public class VerifyEmailVm
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

    }
}
