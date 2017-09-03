namespace HardwareShop.Web.Controllers.StoreManager
{
    using Microsoft.AspNet.Identity;
    using Models.ViewModels.Items;
    using Models.ViewModels.Sale;
    using PagedList;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    [Authorize(Roles = "User")]
    public class SaleController : Controller
    {
        private ISaleService saleService;
        private IHomeService homeService;

        public SaleController(ISaleService saleService, IHomeService homeService)
        {
            this.saleService = saleService;
            this.homeService = homeService;
        }

        [HttpGet]
        [Authorize(Roles = "StoreManager")]
        public ActionResult ListSales(string searchString, int? page)
        {
            var model = new ListSalesViewModel();
            model.Sales = (IPagedList<SalesViewModel>)this.saleService.GetSales(searchString, page);

            return this.View(model);
        }

        [HttpGet]
        [Authorize(Roles = "StoreManager")]
        public ActionResult SaleDetails(int? saleId, int? page)
        {
            if (saleId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SaleDetailsViewModel model = this.saleService.GetSale((int)saleId);

            if (model == null)
            {
                return HttpNotFound();
            }

            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.saleService.GetItemsBySaleId((int)saleId, page);

            return this.View(model);
        }

        [HttpGet]
        public ActionResult CreateNewSale()
        {
            var model = new CreateSaleViewModel();
            return this.View(model);
        }

        [HttpPost]
        public ActionResult CreateNewSale(CreateSaleViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.User.Identity.GetUserId();
            this.saleService.CreateNewSale(userId, model);

            return this.RedirectToAction("Index", "Home");
        }
    }
}