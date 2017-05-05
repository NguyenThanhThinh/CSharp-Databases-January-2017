namespace HardwareShop.Models.ViewModels.Items
{
    using PagedList;

    public class ListItemsViewModel : ViewModelBase
    {
        public IPagedList<ItemViewModel> Items { get; set; }
    }
}
