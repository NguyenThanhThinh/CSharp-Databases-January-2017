namespace HardwareShop.Models.ViewModels.Sale
{
    using PagedList;

    public class ListSalesViewModel : ViewModelBase
    {
        public string SearchString { get; set; }

        public IPagedList<SalesViewModel> Sales { get; set; }
    }
}
