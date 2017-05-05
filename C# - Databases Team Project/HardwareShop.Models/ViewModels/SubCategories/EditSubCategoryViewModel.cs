namespace HardwareShop.Models.ViewModels.SubCategories
{
    using Categories;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditSubCategoryViewModel : ViewModelBase
    {
        public int SubCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public int CategoryId { get; set; }        

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
