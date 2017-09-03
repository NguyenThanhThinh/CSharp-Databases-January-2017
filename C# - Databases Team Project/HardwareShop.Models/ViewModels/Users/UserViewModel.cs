namespace HardwareShop.Models.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public bool IsDeleted { get; set; }
    }
}