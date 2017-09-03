namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using Models.EntityModels;
    using Models.ViewModels.Categories;
    using Models.ViewModels.Items;
    using PagedList;
    using System.Collections.Generic;
    using System.Linq;

    public class HomeService : IHomeService
    {
        public IEnumerable<NavbarCategoriesViewModel> GetCategories()
        {
            using (var context = new HardwareShopContext())
            {
                var categories = context.Categories.Where(c => c.IsDeleted == false).ToList();
                var model = Mapper.Map<IEnumerable<Category>, IEnumerable<NavbarCategoriesViewModel>>(categories);

                return model;
            }
        }

        public IEnumerable<HomeItemsViewModel> GetAllItems(int? page)
        {
            using (var context = new HardwareShopContext())
            {
                var items = context.Items.Where(i => i.IsDeleted == false).ToList();
                var model = Mapper.Map<IEnumerable<Item>, IEnumerable<HomeItemsViewModel>>(items).OrderByDescending(i => i.UploadDate);

                return model.ToPagedList(page ?? 1, 3);
            }
        }

        public IEnumerable<HomeItemsViewModel> GetItemsByCategoryId(int? page, int categoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var items = context.Items.Where(i => i.SubCategory.CategoryId == categoryId && i.IsDeleted == false).ToList();
                var model = Mapper.Map<IEnumerable<Item>, IEnumerable<HomeItemsViewModel>>(items).OrderByDescending(i => i.UploadDate);

                return model.ToPagedList(page ?? 1, 3);
            }
        }

        public IEnumerable<HomeItemsViewModel> GetItemsBySubCategoryId(int? page, int subCategoryId)
        {
            using (var context = new HardwareShopContext())
            {
                var items = context.Items.Where(i => i.SubCategoryId == subCategoryId && i.IsDeleted == false).ToList();
                var model = Mapper.Map<IEnumerable<Item>, IEnumerable<HomeItemsViewModel>>(items).OrderByDescending(i => i.UploadDate);

                return model.ToPagedList(page ?? 1, 3);
            }
        }
    }
}