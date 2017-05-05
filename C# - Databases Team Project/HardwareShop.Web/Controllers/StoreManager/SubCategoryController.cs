namespace HardwareShop.Web.Controllers.StoreManager
{
    using Models.ViewModels.SubCategories;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    [Authorize(Roles = "StoreManager")]
    public class SubCategoryController : Controller
    {
        private ICategoryService categoryService;
        private ISubCategoryService subCategoryService;

        public SubCategoryController(ICategoryService categoryService, ISubCategoryService subCategoryService)
        {
            this.categoryService = categoryService;
            this.subCategoryService = subCategoryService;
        }

        [HttpGet]
        public ActionResult Create()
        {
            var categories = this.categoryService.GetAllCategories();
            var model = new CreateSubCategoryViewModel
            {
                Categories = categories
            };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Create(EditSubCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllCategories();
                return this.View(model);
            }

            this.subCategoryService.AddSubCategory(model);
            return RedirectToAction("List", "Category");
        }

        [HttpGet]
        public ActionResult Edit(int? subCategoryId)
        {
            if (subCategoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.subCategoryService.GetSubCategory((int)subCategoryId);

            if (model == null)
            {
                return HttpNotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditSubCategoryViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                model.Categories = this.categoryService.GetAllCategories();
                return this.View(model);
            }

            this.subCategoryService.EditSubCategory(model);
            return this.RedirectToAction("List", "Category");
        }

        [HttpGet]
        public ActionResult Delete(int? subCategoryId)
        {
            if (subCategoryId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.subCategoryService.GetSubCategoryForDelete((int)subCategoryId);

            if (model == null)
            {
                return HttpNotFound();
            }

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Delete(int subCategoryId)
        {
            this.subCategoryService.Delete(subCategoryId);
            return this.RedirectToAction("List", "Category");
        }

        [HttpPost]
        public ActionResult Restore(int subCategoryId)
        {
            this.subCategoryService.Restore(subCategoryId);
            return this.RedirectToAction("List", "Category");
        }
    }
}