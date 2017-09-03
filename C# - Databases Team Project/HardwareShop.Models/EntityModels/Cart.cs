namespace HardwareShop.Models.EntityModels
{
    using System.Collections.Generic;

    public class Cart
    {
        public Cart()
        {
            this.Items = new HashSet<Item>();
        }

        public int CartId { get; set; }

        public bool IsSold { get; set; }

        public decimal TotalPrice { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}