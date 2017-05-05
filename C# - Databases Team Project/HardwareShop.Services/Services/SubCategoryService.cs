namespace HardwareShop.Services.Services
{
    using System.Linq;
    using Models.EntityModels;
    using AutoMapper;
    using System.Collections.Generic;
    using Models.ViewModels.SubCategories;
    using Models.ViewModels.Categories;
    using Data;
    using Contracts;
    using System.Data.Entity;
    using EntityFramework.Extensions;

    public class SubCategoryService : ISubCategoryService
    {
        public void AddSubCategory(EditSubCategoryViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = Mapper.Map<SubCategory>(model);

                context.SubCategories.Add(subCategory);
                context.SaveChanges();
            }
        }

        public EditSubCategoryViewModel GetSubCategory(int subCategoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = this.GetSubCategoryById(subCategoryId, context);
                var model = Mapper.Map<EditSubCategoryViewModel>(subCategory);
                var categories = context.Categories;
                model.Categories = Mapper.Map<IEnumerable<CategoryViewModel>>(categories);

                return model;
            }
        }

        public void EditSubCategory(EditSubCategoryViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = this.GetSubCategoryById(model.SubCategoryId, context);
                subCategory.CategoryId = model.CategoryId;
                subCategory.Name = model.Name;

                context.Entry(subCategory).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(int subCategoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = GetSubCategoryById(subCategoryId, context);

                subCategory.IsDeleted = true;

                context.Items.Where(i => i.SubCategoryId == subCategory.SubCategoryId).Update(i => new Item { IsDeleted = true });
                context.Reviews.Where(r => r.Item.SubCategoryId == subCategory.SubCategoryId).Update(r => new Review { IsDeleted = true });
                context.Comments.Where(c => c.Review.Item.SubCategoryId == subCategory.SubCategoryId).Update(c => new Comment { IsDeleted = true });
                
                context.Entry(subCategory).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Restore(int subCategoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = this.GetSubCategoryById(subCategoryId, context);

                subCategory.IsDeleted = false;
                context.SaveChanges();
            }
        }

        public DeleteSubCategoryViewModel GetSubCategoryForDelete(int subCategoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var subCategory = this.GetSubCategoryById(subCategoryId, context);

                if (subCategory.IsDeleted == true)
                {
                    return null;
                }

                var subCategoryVm = Mapper.Map<DeleteSubCategoryViewModel>(subCategory);
                return subCategoryVm;
            }
        }

        private SubCategory GetSubCategoryById(int subCategoryId, HardwareShopContext context)
        {
            return context.SubCategories.FirstOrDefault(sc => sc.SubCategoryId == subCategoryId);
        }
    }
}
