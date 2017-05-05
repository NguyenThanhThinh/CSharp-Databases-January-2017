namespace HardwareShop.Web.Controllers.StoreManager
{
    using Models.ViewModels.Categories;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    [Authorize(Roles = "StoreManager")]
    public class CategoryController : Controller
    {
        private ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult List()
        {
            var model = new ListCategoriesViewModel();
            model.Categories = this.categoryService.ListAllCategories();

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Create(CategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.categoryService.AddCategory(model);
            return this.RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Edit(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.categoryService.GetCategory((int)categoryId);

            if (model == null)
            {
                return HttpNotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(CategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.categoryService.EdiCategory(model);
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int? categoryId)
        {
            if (categoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.categoryService.GetCategoryForDelete((int)categoryId);

            if (model == null)
            {
                return HttpNotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int categoryId)
        {
            this.categoryService.Delete(categoryId);
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Restore(int categoryId)
        {
            this.categoryService.Restore(categoryId);
            return this.RedirectToAction("List");
        }
    }
}