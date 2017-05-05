namespace HardwareShop.Models.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class CategoryViewModel : ViewModelBase
    {
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
