namespace HardwareShop.Models.ViewModels.Users
{
    using Role;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class EditUserViewModel : ViewModelBase
    {
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords doesn't match")]
        public string ConfirmPassword { get; set; }

        public IList<RoleViewModel> Roles { get; set; }
    }
}
