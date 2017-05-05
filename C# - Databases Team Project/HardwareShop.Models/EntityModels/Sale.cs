namespace HardwareShop.Models.EntityModels
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Sale
    {
        public Sale()
        {
            this.SaleDate = DateTime.Now.Date;
        }

        [ForeignKey("Cart")]
        public int SaleId { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Email { get; set; }
        
        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime SaleDate { get; set; }

        public virtual Cart Cart { get; set; }
    }
}