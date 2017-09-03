namespace HardwareShop.Services.Contracts
{
    using Models.ViewModels.Items;
    using Models.ViewModels.Sale;
    using System.Collections.Generic;

    public interface ISaleService
    {
        void CreateNewSale(string userId, CreateSaleViewModel model);

        IEnumerable<HomeItemsViewModel> GetItemsBySaleId(int saleId, int? page);

        IEnumerable<SalesViewModel> GetSales(string searchString, int? page);

        SaleDetailsViewModel GetSale(int saleId);
    }
}