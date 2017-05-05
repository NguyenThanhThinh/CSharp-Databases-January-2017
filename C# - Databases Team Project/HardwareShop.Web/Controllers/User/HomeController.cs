namespace HardwareShop.Web.Controllers.User
{
    using Models.ViewModels.Home;
    using Models.ViewModels.Items;
    using PagedList;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        private IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        [HttpGet]
        public ActionResult Index(int? page)
        {
            var model = new HomeViewModel();
            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.homeService.GetAllItems(page);

            return this.View(model);
        }

        [HttpGet]
        public ActionResult ListItemsByCategory(int? categoryId, int? page)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new HomeViewModel();
            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.homeService.GetItemsByCategoryId(page, (int)categoryId);

            if (model.Items == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = categoryId;

            return this.View(model);
        }

        [HttpGet]
        public ActionResult ListItemsBySubCategory(int? subCategoryId, int? page)
        {
            if (subCategoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new HomeViewModel();
            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.homeService.GetItemsBySubCategoryId(page, (int)subCategoryId);

            if (model.Items == null)
            {
                return HttpNotFound();
            }

            ViewBag.SubCategoryId = subCategoryId;

            return this.View(model);
        }
    }
}