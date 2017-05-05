namespace HardwareShop.Models.ViewModels.Users
{
    using PagedList;
    using Search;

    public class ListUsersViewModel : ViewModelBase
    {
        public SearchViewModel SearchViewModel { get; set; }

        public IPagedList<UserViewModel> Users { get; set; }        
    }
}
