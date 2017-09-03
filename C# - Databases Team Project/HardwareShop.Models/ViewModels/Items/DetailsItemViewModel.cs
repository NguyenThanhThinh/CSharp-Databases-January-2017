namespace HardwareShop.Models.ViewModels.Items
{
    using EntityModels;
    using System;

    public class DetailsItemViewModel
    {
        public int ItemId { get; set; }

        public string ManufacturerName { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal NewPrice { get; set; }

        public ItemStatus ItemStatus { get; set; }

        public double Rating { get; set; }

        public int Quantity { get; set; }

        public int WarrantyLengthMonths { get; set; }

        public DateTime UploadDate { get; set; }

        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }

        public string Picture { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSubCategoryDeleted { get; set; }

        public bool IsItemInCart { get; set; }
    }
}