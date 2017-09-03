namespace HardwareShop.Models.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ExternalLoginConfirmationViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}