namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using EntityFramework.Extensions;
    using Models.EntityModels;
    using Models.ViewModels.Categories;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class CategoryService : ICategoryService
    {
        public IEnumerable<CategoriesSubCategoriesViewModel> ListAllCategories()
        {
            using (var context = new HardwareShopContext())
            {
                var categories = context.Categories.ToList();
                var viewModels = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoriesSubCategoriesViewModel>>(categories);

                return viewModels;
            }
        }

        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            using (var context = new HardwareShopContext())
            {
                var categories = context.Categories.ToList();
                var viewModels = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(categories);

                return viewModels;
            }
        }

        public void AddCategory(CategoryViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var newCategory = Mapper.Map<Category>(model);

                context.Categories.Add(newCategory);
                context.SaveChanges();
            }
        }

        public CategoryViewModel GetCategory(int categoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var category = this.GetCategoryById(categoryId, context);
                var viewModel = Mapper.Map<CategoryViewModel>(category);
                return viewModel;
            }
        }

        public void EdiCategory(CategoryViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var category = this.GetCategoryById(model.CategoryId, context);

                category.Name = model.Name;
                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(int categoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var category = this.GetCategoryById(categoryId, context);

                category.IsDeleted = true;

                context.SubCategories.Where(sc => sc.CategoryId == category.CategoryId).Update(c => new SubCategory { IsDeleted = true });
                context.Items.Where(i => i.SubCategory.CategoryId == category.CategoryId).Update(i => new Item { IsDeleted = true });
                context.Reviews.Where(r => r.Item.SubCategory.CategoryId == category.CategoryId).Update(r => new Review { IsDeleted = true });
                context.Comments.Where(c => c.Review.Item.SubCategory.CategoryId == category.CategoryId).Update(c => new Comment { IsDeleted = true });

                context.Entry(category).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Restore(int categoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var category = this.GetCategoryById(categoryId, context);

                category.IsDeleted = false;
                context.SaveChanges();
            }
        }

        private Category GetCategoryById(int categoryId, HardwareShopContext context)
        {
            return context.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public CategoryViewModel GetCategoryForDelete(int categoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var category = this.GetCategoryById(categoryId, context);

                if (category.IsDeleted == true)
                {
                    return null;
                }

                var viewModel = Mapper.Map<CategoryViewModel>(category);
                return viewModel;
            }
        }
    }
}