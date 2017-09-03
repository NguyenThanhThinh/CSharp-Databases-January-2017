namespace HardwareShop.Models.ViewModels.Categories
{
    using SubCategories;
    using System.Collections.Generic;

    public class NavbarCategoriesViewModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public IEnumerable<NavbarSubCategoriesViewModel> SubCategories { get; set; }
    }
}