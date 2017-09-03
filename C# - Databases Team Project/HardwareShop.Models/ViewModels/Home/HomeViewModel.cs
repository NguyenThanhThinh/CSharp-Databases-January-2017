namespace HardwareShop.Models.ViewModels.Home
{
    using Categories;
    using Items;
    using PagedList;
    using System.Collections.Generic;

    public class HomeViewModel : ViewModelBase
    {
        public IEnumerable<NavbarCategoriesViewModel> Categories { get; set; }

        public IPagedList<HomeItemsViewModel> Items { get; set; }
    }
}