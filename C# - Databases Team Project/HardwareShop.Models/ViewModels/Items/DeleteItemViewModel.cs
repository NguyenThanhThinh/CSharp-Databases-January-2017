namespace HardwareShop.Models.ViewModels.Items
{
    using EntityModels;
    using System;
    using System.ComponentModel;

    public class DeleteItemViewModel : ViewModelBase
    {
        public int ItemId { get; set; }

        [DisplayName("Manufacturer")]
        public string ManufacturerName { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public Nullable<decimal> NewPrice { get; set; }

        public int Quantity { get; set; }

        public double Rateing { get; set; }

        public ItemStatus ItemStatus { get; set; }

        [DisplayName("Warranty")]
        public int WarrantyLengthMonths { get; set; }

        [DisplayName("Created")]
        public DateTime UploadDate { get; set; }

        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [DisplayName("Subcategory")]
        public string SubCategoryName { get; set; }

        public string Description { get; set; }
    }
}