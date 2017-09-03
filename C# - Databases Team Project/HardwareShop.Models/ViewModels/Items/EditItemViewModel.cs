namespace HardwareShop.Models.ViewModels.Items
{
    using EntityModels;
    using SubCategories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class EditItemViewModel : ViewModelBase
    {
        public int ItemId { get; set; }

        [Required]
        [DisplayName("Manufacturer")]
        public string ManufacturerName { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public ItemStatus ItemStatus { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1.0, Double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1.0, Double.MaxValue)]
        public Nullable<decimal> NewPrice { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 100)]
        [DisplayName("Warranty")]
        public int WarrantyLengthMonths { get; set; }

        public string Picture { get; set; }

        [DisplayName("Upload New Picture")]
        public HttpPostedFileBase PictureBase { get; set; }

        public int SubCategoryId { get; set; }

        public IEnumerable<EditSubCategoryViewModel> SubCategories { get; set; }
    }
}