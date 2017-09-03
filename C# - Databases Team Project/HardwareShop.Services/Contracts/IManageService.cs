namespace HardwareShop.Services.Contracts
{
    using HardwareShop.Models.ViewModels.Manage;

    public interface IManageService
    {
        void EditUser(ManageUserViewModel model);

        ManageUserViewModel GetUser(string username);
    }
}