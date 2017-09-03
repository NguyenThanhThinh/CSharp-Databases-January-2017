namespace HardwareShop.Models.ViewModels.Categories
{
    using System.Collections.Generic;

    public class ListCategoriesViewModel : ViewModelBase
    {
        public IEnumerable<CategoriesSubCategoriesViewModel> Categories { get; set; }
    }
}