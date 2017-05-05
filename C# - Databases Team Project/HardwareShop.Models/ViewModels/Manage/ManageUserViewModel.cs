namespace HardwareShop.Models.ViewModels.Manage
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class ManageUserViewModel : ViewModelBase
    {
        public string Id { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
