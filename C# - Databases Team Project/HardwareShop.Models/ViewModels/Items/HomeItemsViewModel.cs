namespace HardwareShop.Models.ViewModels.Items
{
    using System;

    public class HomeItemsViewModel
    {
        public int ItemId { get; set; }

        public string ManufacturerName { get; set; }

        public string Model { get; set; }

        public int WarrantyLengthMonths { get; set; }

        public string ItemStatus { get; set; }

        public decimal Price { get; set; }

        public DateTime UploadDate { get; set; }

        public Nullable<decimal> NewPrice { get; set; }

        public string Picture { get; set; }
    }
}