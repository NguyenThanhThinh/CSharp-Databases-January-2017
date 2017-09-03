namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using EntityFramework.Extensions;
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using Models.ViewModels.Sale;
    using PagedList;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SaleService : ISaleService
    {
        public void CreateNewSale(string userId, CreateSaleViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Id == userId);
                var cart = context.Carts.FirstOrDefault(c => c.UserId == userId && c.IsSold == false);
                var sale = Mapper.Instance.Map<CreateSaleViewModel, Sale>(model);

                sale.Cart = cart;
                sale.SaleDate = DateTime.Now;
                sale.TotalPrice = cart.Items.Sum(i => i.Price);

                cart.IsSold = true;

                foreach (var item in cart.Items)
                {
                    item.Quantity--;
                }

                user.Carts.Add(new Cart());
                context.Sales.Add(sale);
                context.SaveChanges();

                context.Items.Where(i => i.Quantity < 1).Update(i => new Item { ItemStatus = ItemStatus.OutOfStock });
                context.SaveChanges();
            }
        }

        public IEnumerable<SalesViewModel> GetSales(string searchString, int? page)
        {
            using (var context = new HardwareShopContext())
            {
                searchString = searchString ?? string.Empty;
                IEnumerable<Sale> sales = context.Sales.Where(s => s.Cart.User.UserName.Contains(searchString));
                IEnumerable<SalesViewModel> viewModel = Mapper.Map<IEnumerable<Sale>, IEnumerable<SalesViewModel>>(sales);

                return viewModel.ToPagedList(page ?? 1, 3);
            }
        }

        public IEnumerable<HomeItemsViewModel> GetItemsBySaleId(int saleId, int? page)
        {
            using (var context = new HardwareShopContext())
            {
                var items = context.Sales.FirstOrDefault(s => s.SaleId == saleId).Cart.Items;
                var model = Mapper.Map<IEnumerable<Item>, IEnumerable<HomeItemsViewModel>>(items);

                return model.ToPagedList(page ?? 1, 3);
            }
        }

        public SaleDetailsViewModel GetSale(int saleId)
        {
            using (var context = new HardwareShopContext())
            {
                Sale sale = context.Sales.FirstOrDefault(s => s.SaleId == saleId);
                SaleDetailsViewModel model = Mapper.Instance.Map<Sale, SaleDetailsViewModel>(sale);

                return model;
            }
        }
    }
}