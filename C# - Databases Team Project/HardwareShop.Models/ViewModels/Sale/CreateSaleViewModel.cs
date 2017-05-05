namespace HardwareShop.Models.ViewModels.Sale
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateSaleViewModel : ViewModelBase
    {
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
