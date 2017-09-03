namespace HardwareShop.Models.ViewModels.Items
{
    using PagedList;
    using Reviews;

    public class DetailsItemPageViewModel : ViewModelBase
    {
        public DetailsItemViewModel Item { get; set; }

        public IPagedList<ListReviewsViewModel> Reviews { get; set; }
    }
}