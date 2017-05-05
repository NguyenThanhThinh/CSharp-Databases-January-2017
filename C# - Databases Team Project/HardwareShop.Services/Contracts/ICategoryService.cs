namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;
    using Models.ViewModels.Categories;

    public interface ICategoryService
    {
        void AddCategory(CategoryViewModel model);
        void Delete(int categoryId);
        void EdiCategory(CategoryViewModel model);
        IEnumerable<CategoryViewModel> GetAllCategories();
        CategoryViewModel GetCategory(int categoryId);
        IEnumerable<CategoriesSubCategoriesViewModel> ListAllCategories();
        void Restore(int categoryId);
        CategoryViewModel GetCategoryForDelete(int categoryId);
    }
}