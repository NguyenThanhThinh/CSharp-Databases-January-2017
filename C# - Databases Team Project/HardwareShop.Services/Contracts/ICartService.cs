namespace HardwareShop.Services.Contracts
{
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using System.Collections.Generic;

    public interface ICartService
    {
        void AddItemToCart(Item item, string userId);

        void ClearItemsFromCart(string userId);

        IEnumerable<HomeItemsViewModel> GetItemsByUser(string userId, int? page);

        decimal GetTotalPrice(string userId);

        void RemoveItemFromCart(Item item, string userId);
    }
}