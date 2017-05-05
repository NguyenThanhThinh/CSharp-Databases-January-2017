namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using PagedList;
    using System.Collections.Generic;
    using System.Linq;

    public class CartService : ICartService
    {
        public IEnumerable<HomeItemsViewModel> GetItemsByUser(string userId, int? page)
        {
            using (var context = new HardwareShopContext())
            {
                var items = context.Carts.FirstOrDefault(s => s.User.Id == userId && s.IsSold == false).Items;
                var model = Mapper.Map<IEnumerable<Item>, IEnumerable<HomeItemsViewModel>>(items);

                return model.ToPagedList(page ?? 1, 3);
            }
        }

        public void ClearItemsFromCart(string userId)
        {
            using (var context = new HardwareShopContext())
            {
                context.Carts.FirstOrDefault(s => s.UserId == userId && s.IsSold == false).Items.Clear();
                context.SaveChanges();
            }
        }

        public void AddItemToCart(Item item, string userId)
        {
            using (var context = new HardwareShopContext())
            {
                context.Items.Attach(item);
                Cart cart = context.Carts.FirstOrDefault(s => s.UserId == userId && s.IsSold == false);
                cart.Items.Add(item);
                cart.TotalPrice += item.Price;
                context.SaveChanges();
            }
        }

        public void RemoveItemFromCart(Item item, string userId)
        {
            using (var context = new HardwareShopContext())
            {
                context.Items.Attach(item);
                Cart cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.IsSold == false);
                cart.Items.Remove(item);
                cart.TotalPrice -= item.Price;
                context.SaveChanges();
            }
        }

        public decimal GetTotalPrice(string userId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Carts.FirstOrDefault(s => s.UserId == userId && s.IsSold == false).TotalPrice;
            }
        }
    }
}
