namespace HardwareShop.Models.ViewModels.SubCategories
{
    using Categories;
    using System.Collections.Generic;

    public class CreateSubCategoryViewModel : ViewModelBase
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}