namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;
    using Models.ViewModels.Categories;
    using Models.ViewModels.Items;

    public interface IHomeService
    {
        IEnumerable<HomeItemsViewModel> GetAllItems(int? page);
        IEnumerable<NavbarCategoriesViewModel> GetCategories();
        IEnumerable<HomeItemsViewModel> GetItemsByCategoryId(int? page, int categoryId);
        IEnumerable<HomeItemsViewModel> GetItemsBySubCategoryId(int? page, int subCategoryId);
    }
}