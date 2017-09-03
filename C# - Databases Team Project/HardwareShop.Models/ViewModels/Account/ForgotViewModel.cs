namespace HardwareShop.Models.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotViewModel : ViewModelBase
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}