namespace HardwareShop.Services.Contracts
{
    using Models.EntityModels;
    using Models.ViewModels.Search;
    using Models.ViewModels.Users;
    using System.Collections.Generic;

    public interface IUserService
    {
        ApplicationUser GetUser(string userId);
        IEnumerable<UserViewModel> GetUsersForList(int? page, SearchViewModel model);
        EditUserViewModel GetUserForEdit(string userId);
        void EditUser(ApplicationUser user, EditUserViewModel model);
        DeleteUserViewModel GetUserForDelete(string userId);
    }
}