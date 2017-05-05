namespace HardwareShop.Models.ViewModels.Items
{
    using EntityModels;
    using PagedList;
    using Reviews;
    using System;

    public class DetailsItemPageViewModel : ViewModelBase
    {
        public DetailsItemViewModel Item { get; set; }

        public IPagedList<ListReviewsViewModel> Reviews { get; set; }
    }
}
