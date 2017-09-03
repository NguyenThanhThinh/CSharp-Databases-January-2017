namespace HardwareShop.Models.ViewModels.Items
{
    using System;

    public class ItemViewModel
    {
        public int ItemId { get; set; }

        public string ManufacturerName { get; set; }

        public string Model { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Nullable<decimal> NewPrice { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSubCategoryDeleted { get; set; }
    }
}