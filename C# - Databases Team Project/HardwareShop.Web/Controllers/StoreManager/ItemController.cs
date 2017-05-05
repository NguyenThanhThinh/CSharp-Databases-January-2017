namespace HardwareShop.Web.Controllers.StoreManager
{
    using System.Net;
    using System.Web.Mvc;
    using Models.ViewModels.Items;
    using Services.Contracts;
    using Models.ViewModels.Reviews;
    using PagedList;
    using Microsoft.AspNet.Identity;

    [Authorize(Roles = "StoreManager")]
    public class ItemController : Controller
    {
        private IItemService itemService;
        private IReviewService reviewService;

        public ItemController(IItemService itemService, IReviewService reviewService)
        {
            this.itemService = itemService;
            this.reviewService = reviewService;
        }

        // GET: Item/List
        [HttpGet]
        public ActionResult ListDeletedItems(ListItemsViewModel itemsModel, int? page)
        {
            var model = itemsModel ?? new ListItemsViewModel();
            model.Items = this.itemService.GetDeletedItems(itemsModel.QueryString, page);

            return this.View(model);
        }

        // GET: Item/Details/id
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Details(int? page, int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new DetailsItemPageViewModel();

            var item = itemService.GetItemDetailsById(itemId);

            if (item == null || item.IsDeleted)
            {
                return HttpNotFound();
            }

            string userId = this.User.Identity.GetUserId();

            model.Item = item;
            model.Item.IsItemInCart = this.itemService.IsItemInCart((int)itemId, userId);
            model.Reviews = (IPagedList<ListReviewsViewModel>)this.reviewService.GetReviewsByItemId(page, (int)itemId);

            return View(model);
        }

        // GET: Item/Create
        [HttpGet]
        public ActionResult Create()
        {
            var subCategories = this.itemService.GetAllSubCategories();
            var model = new CreateItemViewModel()
            {
                SubCategories = subCategories
            };

            return this.View(model);
        }

        // POST: Item/Create
        [HttpPost]
        public ActionResult Create(CreateItemViewModel createModel)
        {
            if (!this.ModelState.IsValid)
            {
                createModel.SubCategories = this.itemService.GetAllSubCategories();
                return this.View(createModel);
            }

            this.itemService.AddItem(createModel);
            return this.RedirectToAction("Index", "Home");
        }

        // GET: Item/Edit/id
        [HttpGet]
        public ActionResult Edit(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.itemService.GetEditItemViewModel((int)itemId);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorizedToEdit(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Item/Edit/id
        [HttpPost]
        public ActionResult Edit(EditItemViewModel editModel)
        {
            if (!this.ModelState.IsValid)
            {
                editModel.SubCategories = this.itemService.GetAllSubCategories();
                return this.View(editModel);
            }

            this.itemService.EditItem(editModel);
            return this.RedirectToAction("Details", "Item", new { itemId = editModel.ItemId });
        }

        // GET: Item/Delete/id
        [HttpGet]
        public ActionResult Delete(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.itemService.GetItemForDelete((int)itemId);

            if (model == null)
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorizedToDelete(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Item/Delete/id
        [HttpPost]
        public ActionResult Delete(int itemId)
        {
            this.itemService.DeleteItem((int)itemId);
            return this.RedirectToAction("ListDeletedItems");
        }

        [HttpPost]
        public ActionResult Restore(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!this.itemService.IsItemExisting((int)itemId))
            {
                return HttpNotFound();
            }

            this.itemService.Restore((int)itemId);

            return this.RedirectToAction("ListDeletedItems");
        }

        private bool IsUserAuthorizedToEdit(EditItemViewModel editItemViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isStoreManager = this.User.IsInRole("StoreManager");

            return isAdmin || isStoreManager;
        }

        private bool IsUserAuthorizedToDelete(DeleteItemViewModel deleteItemViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isStoreManager = this.User.IsInRole("StoreManager");

            return isAdmin || isStoreManager;
        }
    }
}
