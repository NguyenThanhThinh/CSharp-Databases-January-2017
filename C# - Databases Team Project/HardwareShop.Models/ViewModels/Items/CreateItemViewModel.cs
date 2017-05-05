namespace HardwareShop.Models.ViewModels.Items
{
    using System;
    using System.Web;
    using SubCategories;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using EntityModels;

    public class CreateItemViewModel : ViewModelBase
    {
        [Required]
        [DisplayName("Manufacturer")]
        public string ManufacturerName { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.0, Double.MaxValue)]
        public decimal Price { get; set; }

        public ItemStatus ItemStatus { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(1, 100)]
        [DisplayName("Warranty")]
        public int WarrantyLengthMonths { get; set; }

        [DisplayName("Upload Picture")]
        [Required]
        public HttpPostedFileBase Picture { get; set; }

        public int SubCategoryId { get; set; }

        public IEnumerable<EditSubCategoryViewModel> SubCategories { get; set; }
    }
}
