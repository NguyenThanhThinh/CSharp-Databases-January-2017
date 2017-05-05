namespace HardwareShop.Models.ViewModels.SubCategories
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteSubCategoryViewModel : ViewModelBase
    {
        public int SubCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string CategoryName { get; set; }
    }
}
