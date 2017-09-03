namespace HardwareShop.Models.ViewModels.Cart
{
    using Categories;
    using Items;
    using PagedList;
    using System.Collections.Generic;

    public class ListItemsInCartViewModel : ViewModelBase
    {
        public decimal TotalPrice { get; set; }

        public IEnumerable<NavbarCategoriesViewModel> Categories { get; set; }

        public IPagedList<HomeItemsViewModel> Items { get; set; }
    }
}