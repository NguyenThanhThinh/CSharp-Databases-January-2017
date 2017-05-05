namespace HardwareShop.Models.ViewModels.Sale
{
    using System;

    public class SalesViewModel
    {
        public int SaleId { get; set; }

        public string Username { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime SaleDate { get; set; }
    }
}
