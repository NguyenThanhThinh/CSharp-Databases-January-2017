namespace HardwareShop.Models.EntityModels
{
    using System;
    using System.Collections.Generic;

    public enum ItemStatus
    {
        Available,
        OutOfStock,
        Promotion,
        New
    }

    public class Item
    {
        public Item()
        {
            this.Carts = new HashSet<Cart>();
            this.Reviews = new HashSet<Review>();
            this.UploadDate = DateTime.Now;
        }

        public int ItemId { get; set; }

        public string ManufacturerName { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public Nullable<decimal> NewPrice { get; set; }

        public int Quantity { get; set; }

        public int WarrantyLengthMonths { get; set; }

        public byte[] Picture { get; set; }

        public DateTime UploadDate { get; set; }

        public bool IsDeleted { get; set; }

        public ItemStatus ItemStatus { get; set; }

        public int SubCategoryId { get; set; }

        public virtual SubCategory SubCategory { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}