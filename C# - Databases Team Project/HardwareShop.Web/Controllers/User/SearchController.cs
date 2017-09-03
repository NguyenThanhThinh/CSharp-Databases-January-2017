namespace HardwareShop.Web.Controllers.User
{
    using Models.ViewModels;
    using Models.ViewModels.Home;
    using Models.ViewModels.Items;
    using PagedList;
    using Services.Contracts;
    using System.Web.Mvc;

    public class SearchController : Controller
    {
        private ISearchService searchService;
        private IHomeService homeService;

        public SearchController(ISearchService searchService, IHomeService homeService)
        {
            this.searchService = searchService;
            this.homeService = homeService;
        }

        [HttpGet]
        public ActionResult Search(ViewModelBase searchModel, int? page)
        {
            var model = new HomeViewModel();

            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.searchService.SearchItems(searchModel.QueryString, page);

            return this.View(model);
        }
    }
}