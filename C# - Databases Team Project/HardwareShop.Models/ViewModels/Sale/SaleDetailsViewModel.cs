namespace HardwareShop.Models.ViewModels.Sale
{
    using Categories;
    using Items;
    using PagedList;
    using System.Collections.Generic;

    public class SaleDetailsViewModel : ViewModelBase
    {
        public int SaleId { get; set; }

        public string Username { get; set; }

        public decimal TotalPrice { get; set; }

        public string SaleDate { get; set; }

        public IEnumerable<NavbarCategoriesViewModel> Categories { get; set; }

        public IPagedList<HomeItemsViewModel> Items { get; set; }
    }
}
