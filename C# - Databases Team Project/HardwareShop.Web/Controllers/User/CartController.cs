namespace HardwareShop.Web.Controllers.User
{
    using Microsoft.AspNet.Identity;
    using Models.ViewModels.Cart;
    using Models.ViewModels.Items;
    using PagedList;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    public class CartController : Controller
    {
        private ICartService cartService;
        private IHomeService homeService;
        private IItemService itemService;

        public CartController(ICartService cartService, IHomeService homeService, IItemService itemService)
        {
            this.cartService = cartService;
            this.homeService = homeService;
            this.itemService = itemService;
        }

        [HttpGet]
        public ActionResult ListItemsInCart(int? page)
        {
            ListItemsInCartViewModel model = new ListItemsInCartViewModel();

            string userId = this.User.Identity.GetUserId();
            model.Categories = this.homeService.GetCategories();
            model.Items = (IPagedList<HomeItemsViewModel>)this.cartService.GetItemsByUser(userId, page);
            model.TotalPrice = this.cartService.GetTotalPrice(userId);

            return this.View(model);
        }

        [HttpPost]
        public ActionResult AddItemToCart(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = this.itemService.GetItem((int)itemId);

            if (item == null)
            {
                return HttpNotFound();
            }

            string userId = this.User.Identity.GetUserId();

            if (this.itemService.IsItemInCart((int)itemId, userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.cartService.AddItemToCart(item, userId);

            return this.RedirectToAction("Details", "Item", new { itemId = item.ItemId });
        }

        [HttpPost]
        public ActionResult RemoveItemFromCart(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var item = this.itemService.GetItem((int)itemId);

            if (item == null)
            {
                return HttpNotFound();
            }

            string userId = this.User.Identity.GetUserId();

            if (!this.itemService.IsItemInCart((int)itemId, userId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.cartService.RemoveItemFromCart(item, userId);

            return this.RedirectToAction("ListItemsInCart");
        }

        [HttpPost]
        public ActionResult ClearItemsFromCart()
        {
            var userId = this.User.Identity.GetUserId();
            this.cartService.ClearItemsFromCart(userId);
            return this.RedirectToAction("ListItemsInCart");
        }
    }
}