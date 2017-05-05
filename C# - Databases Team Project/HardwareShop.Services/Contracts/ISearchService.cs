namespace HardwareShop.Services.Contracts
{
    using System.Collections.Generic;
    using Models.ViewModels.Items;

    public interface ISearchService
    {
        IEnumerable<HomeItemsViewModel> SearchItems(string searchString, int? page);
    }
}