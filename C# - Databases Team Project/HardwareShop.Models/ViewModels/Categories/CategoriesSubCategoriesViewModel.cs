namespace HardwareShop.Models.ViewModels.Categories
{
    using SubCategories;
    using System.Collections.Generic;

    public class CategoriesSubCategoriesViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public IEnumerable<SubCategoryViewModel> SubCategories { get; set; }
    }
}
