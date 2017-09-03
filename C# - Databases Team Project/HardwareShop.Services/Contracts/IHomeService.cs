namespace HardwareShop.Services.Contracts
{
    using Models.ViewModels.Categories;
    using Models.ViewModels.Items;
    using System.Collections.Generic;

    public interface IHomeService
    {
        IEnumerable<HomeItemsViewModel> GetAllItems(int? page);

        IEnumerable<NavbarCategoriesViewModel> GetCategories();

        IEnumerable<HomeItemsViewModel> GetItemsByCategoryId(int? page, int categoryId);

        IEnumerable<HomeItemsViewModel> GetItemsBySubCategoryId(int? page, int subCategoryId);
    }
}