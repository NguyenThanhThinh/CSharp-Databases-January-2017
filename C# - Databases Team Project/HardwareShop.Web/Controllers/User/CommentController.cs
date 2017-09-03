namespace HardwareShop.Web.Controllers.User
{
    using Models.ViewModels.Comments;
    using Services.Contracts;
    using System.Net;
    using System.Web.Mvc;

    [Authorize(Roles = "User")]
    public class CommentController : Controller
    {
        private ICommentService commentService;
        private IReviewService reviewService;

        public CommentController(ICommentService commentService, IReviewService reviewService)
        {
            this.commentService = commentService;
            this.reviewService = reviewService;
        }

        // GET: Comment/Create
        [HttpGet]
        public ActionResult Create(int? reviewId, int? itemId)
        {
            if (reviewId == null || itemId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (!this.reviewService.IsItemExisting((int)itemId) || !this.reviewService.IsItemDeleted((int)itemId) ||
                !this.reviewService.IsReviewDeleted((int)reviewId))
            {
                return HttpNotFound();
            }

            var model = new CommentViewModel();
            model.ItemId = (int)itemId;

            return this.View(model);
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(CommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.commentService.AddComment(model);
            return RedirectToAction("Details", "Item", new { itemId = model.ItemId });
        }

        // GET: Comment/Edit/id
        [HttpGet]
        public ActionResult Edit(int? commentId)
        {
            if (commentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.commentService.GetCommentViewModelById((int)commentId);

            if (model == null || !this.reviewService.IsItemExisting((int)model.ItemId)
                || !this.reviewService.IsItemDeleted((int)model.ItemId) || !this.reviewService.IsReviewDeleted((int)model.ReviewId))
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorizedToEdit(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Comment/Edit/id
        [HttpPost]
        public ActionResult Edit(CommentViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            this.commentService.EditComment(model);
            return RedirectToAction("Details", "Item", new { itemId = model.ItemId });
        }

        // GET: Comment/Delete/id
        [HttpGet]
        public ActionResult Delete(int? commentId)
        {
            if (commentId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = this.commentService.GetCommentForDelete((int)commentId);

            if (model == null || !this.reviewService.IsItemExisting((int)model.ItemId)
               || !this.reviewService.IsItemDeleted((int)model.ItemId) || !this.reviewService.IsReviewDeleted((int)model.ReviewId))
            {
                return HttpNotFound();
            }

            if (!IsUserAuthorizedToDelete(model))
            {
                return new HttpUnauthorizedResult();
            }

            return this.View(model);
        }

        // POST: Comment/Delete/id
        [HttpPost]
        public ActionResult Delete(DeleteCommentViewModel viewModel)
        {
            this.commentService.DeleteComment(viewModel.CommentId);
            return RedirectToAction("Details", "Item", new { itemId = viewModel.ItemId });
        }

        private bool IsUserAuthorizedToEdit(CommentViewModel commentViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = commentViewModel.IsAuthor();

            return isAdmin || isAuthor;
        }

        private bool IsUserAuthorizedToDelete(DeleteCommentViewModel deleteCommentViewModel)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = deleteCommentViewModel.IsAuthor();

            return isAdmin || isAuthor;
        }
    }
}