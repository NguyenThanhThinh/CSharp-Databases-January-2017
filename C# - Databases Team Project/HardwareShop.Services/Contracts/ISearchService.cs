namespace HardwareShop.Services.Contracts
{
    using Models.ViewModels.Items;
    using System.Collections.Generic;

    public interface ISearchService
    {
        IEnumerable<HomeItemsViewModel> SearchItems(string searchString, int? page);
    }
}