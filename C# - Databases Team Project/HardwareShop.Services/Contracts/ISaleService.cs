namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;
    using Models.ViewModels.Items;
    using Models.ViewModels.Sale;

    public interface ISaleService
    {
        void CreateNewSale(string userId, CreateSaleViewModel model);
        IEnumerable<HomeItemsViewModel> GetItemsBySaleId(int saleId, int? page);
        IEnumerable<SalesViewModel> GetSales(string searchString, int? page);
        SaleDetailsViewModel GetSale(int saleId);
    }
}