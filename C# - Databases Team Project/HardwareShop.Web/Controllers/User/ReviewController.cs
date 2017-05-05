namespace HardwareShop.Web.Controllers.User
{
    using System.Net;
    using System.Web.Mvc;
    using Models.ViewModels.Reviews;
    using Services.Contracts;

    [Authorize(Roles = "User")]
    public class ReviewController : Controller
    {
        private IReviewService reviewService;

        public ReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        // GET: Review/Create
        [HttpGet]
        public ActionResult Create(int? itemId)
        {
            if (itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!this.reviewService.IsItemExisting((int)itemId) || !this.reviewService.IsItemDeleted((int)itemId))
            {
                return HttpNotFound();
            }

            var model = new CreateReviewViewModel();
            model.ItemId = (int)itemId;
            return this.View(model);
        }

        // POST: Review/Create
        [HttpPost]
        public ActionResult Create(CreateReviewViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.reviewService.AddReview(model);
            return RedirectToAction("Details", "Item", new { itemId = model.ItemId });
        }

        // GET: Review/Edit/id
        [HttpGet]
        public ActionResult Edit(int? reviewId)
        {
            if (reviewId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.reviewService.GetReviewForEdit((int)reviewId);

            if (model == null || !this.reviewService.IsReviewDeleted((int)reviewId))
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorizedToEdit(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Review/Edit/id
        [HttpPost]
        public ActionResult Edit(EditReviewViewModel model, int reviewId)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.reviewService.EditReview(model);
            return this.RedirectToAction("Details", "Item", new { @itemId = model.ItemId});
        }

        // GET: Review/Delete/id
        [HttpGet]
        public ActionResult Delete(int? reviewId)
        {
            if (reviewId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.reviewService.GetReviewForDelete((int)reviewId);

            if (model == null || !this.reviewService.IsReviewDeleted((int)reviewId))
            {
                return HttpNotFound();
            }
            
            if (!IsUserAuthorizedToDelete(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Review/Delete/id
        [HttpPost]
        public ActionResult Delete(DeleteReviewViewModel viewModel)
        {
            this.reviewService.DeleteReview(viewModel.ReviewId);
            return this.RedirectToAction("Details", "Item", new { itemId = viewModel.ItemId });
        }

        private bool IsUserAuthorizedToEdit(EditReviewViewModel reviewViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = reviewViewModel.IsAuthor();

            return isAdmin || isAuthor;
        }

        private bool IsUserAuthorizedToDelete(DeleteReviewViewModel deleteReviewViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = deleteReviewViewModel.IsAuthor();

            return isAdmin || isAuthor;
        }
    }
}
