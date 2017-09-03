namespace HardwareShop.Services.Contracts
{
    using Models.ViewModels.SubCategories;

    public interface ISubCategoryService
    {
        void AddSubCategory(EditSubCategoryViewModel model);

        void Delete(int subCategoryId);

        void EditSubCategory(EditSubCategoryViewModel model);

        EditSubCategoryViewModel GetSubCategory(int subCategoryId);

        DeleteSubCategoryViewModel GetSubCategoryForDelete(int subCategoryId);

        void Restore(int subCategoryId);
    }
}