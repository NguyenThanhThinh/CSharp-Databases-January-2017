namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using EntityFramework.Extensions;
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using Models.ViewModels.SubCategories;
    using PagedList;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class ItemService : IItemService
    {
        public IPagedList<ItemViewModel> GetDeletedItems(string queryString, int? page)
        {
            using (var context = new HardwareShopContext())
            {
                var items = new List<Item>();

                if (queryString != null)
                {
                    items = context.Items
                    .Where(i => i.IsDeleted == true && (i.ManufacturerName.Contains(queryString) ||
                        i.Description.Contains(queryString) ||
                        i.Model.Contains(queryString)))
                    .OrderByDescending(i => i.UploadDate)
                    .ToList();
                }
                else
                {
                    items = context.Items
                        .Where(i => i.IsDeleted == true)
                        .OrderByDescending(i => i.UploadDate)
                        .ToList();
                }

                var viewModels = Mapper.Map<IEnumerable<Item>, IEnumerable<ItemViewModel>>(items);

                return viewModels.ToPagedList(page ?? 1, 3);
            }
        }

        public Item GetItem(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Items.Find(itemId);
            }
        }

        public void AddItem(CreateItemViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var newItem = Mapper.Map<Item>(model);

                context.Items.Add(newItem);
                context.SaveChanges();
            }
        }

        public IEnumerable<EditSubCategoryViewModel> GetAllSubCategories()
        {
            using (var context = new HardwareShopContext())
            {
                var subCategories = context.SubCategories;
                var viewModels = Mapper.Map<IEnumerable<SubCategory>, IEnumerable<EditSubCategoryViewModel>>(subCategories);

                return viewModels;
            }
        }

        public void EditItem(EditItemViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var item = this.GetItemById(model.ItemId, context);

                item.SubCategoryId = model.SubCategoryId;
                item.Description = model.Description;
                item.Quantity = model.Quantity;
                item.Price = model.Price;
                item.NewPrice = model.NewPrice;
                item.Model = model.Model;
                item.ItemStatus = model.ItemStatus;
                item.ManufacturerName = model.ManufacturerName;
                item.WarrantyLengthMonths = model.WarrantyLengthMonths;

                if (model.PictureBase != null)
                {
                    MemoryStream stream = new MemoryStream();
                    model.PictureBase.InputStream.CopyTo(stream);
                    byte[] pictureInData = stream.ToArray();

                    item.Picture = pictureInData;
                }

                context.SaveChanges();
            }
        }

        public EditItemViewModel GetEditItemViewModel(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                var item = this.GetItemById(itemId, context);

                if (item == null)
                {
                    return null;
                }

                var model = Mapper.Map<EditItemViewModel>(item);
                var subCategories = context.SubCategories;
                model.SubCategories = Mapper.Map<IEnumerable<EditSubCategoryViewModel>>(subCategories);

                return model;
            }
        }

        public Item GetItemById(int? itemId, HardwareShopContext context)
        {
            return context.Items
                .FirstOrDefault(i => i.ItemId == itemId);
        }

        public DetailsItemViewModel GetItemDetailsById(int? itemId)
        {
            using (var context = new HardwareShopContext())
            {
                Item item = this.GetItemById(itemId, context);
                DetailsItemViewModel model = Mapper.Instance.Map<Item, DetailsItemViewModel>(item);

                return model;
            }
        }

        public void DeleteItem(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                var item = this.GetItemById(itemId, context);
                item.IsDeleted = true;

                context.Reviews.Where(r => r.ItemId == item.ItemId).Update(r => new Review { IsDeleted = true });
                context.Comments.Where(c => c.Review.Item.ItemId == item.ItemId).Update(c => new Comment { IsDeleted = true });

                context.SaveChanges();
            }
        }

        public void Restore(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                var item = this.GetItemById(itemId, context);
                item.IsDeleted = false;

                context.Reviews.Where(r => r.ItemId == item.ItemId).Update(r => new Review { IsDeleted = false });
                context.Comments.Where(c => c.Review.Item.ItemId == item.ItemId).Update(c => new Comment { IsDeleted = false });

                context.SaveChanges();
            }
        }

        public DeleteItemViewModel GetItemForDelete(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                var item = this.GetItemById(itemId, context);

                if (item.IsDeleted == true)
                {
                    return null;
                }

                var model = Mapper.Map<DeleteItemViewModel>(item);
                return model;
            }
        }

        public bool IsItemExisting(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Items.Any(i => i.ItemId == itemId);
            }
        }

        public bool IsItemInCart(int itemId, string userId)
        {
            using (var context = new HardwareShopContext())
            {
                Item item = context.Items
                    .FirstOrDefault(i => i.ItemId == itemId);

                return context.Carts
                    .FirstOrDefault(c => c.UserId == userId && c.IsSold == false)
                    .Items
                    .Contains(item);
            }
        }
    }
}