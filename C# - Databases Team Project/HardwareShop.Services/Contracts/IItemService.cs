namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;
    using Data;
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using Models.ViewModels.SubCategories;
    using PagedList;

    public interface IItemService
    {
        void AddItem(CreateItemViewModel model);
        void DeleteItem(int itemId);
        void EditItem(EditItemViewModel model);
        IEnumerable<EditSubCategoryViewModel> GetAllSubCategories();
        EditItemViewModel GetEditItemViewModel(int itemId);
        Item GetItemById(int? itemId, HardwareShopContext context);
        Item GetItem(int itemId);
        DetailsItemViewModel GetItemDetailsById(int? itemId);
        DeleteItemViewModel GetItemForDelete(int itemId);
        IPagedList<ItemViewModel> GetDeletedItems(string queryString, int? page);
        void Restore(int itemId);
        bool IsItemExisting(int itemId);
        bool IsItemInCart(int itemId, string userId);
    }
}