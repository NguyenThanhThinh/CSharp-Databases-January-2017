namespace HardwareShop.Services.Contracts
{
    using Data;
    using Models.EntityModels;
    using Models.ViewModels.Reviews;
    using System.Collections.Generic;

    public interface IReviewService
    {
        void AddReview(CreateReviewViewModel model);

        void DeleteReview(int reviewId);

        void EditReview(EditReviewViewModel model);

        Review GetReviewById(int? reviewId, HardwareShopContext context);

        DeleteReviewViewModel GetReviewForDelete(int reviewId);

        CreateReviewViewModel GetReviewViewModelById(int reviewId);

        EditReviewViewModel GetReviewForEdit(int reviewId);

        bool IsItemExisting(int itemId);

        bool IsItemDeleted(int itemId);

        bool IsReviewDeleted(int reviewId);

        IEnumerable<ListReviewsViewModel> GetReviewsByItemId(int? page, int itemId);
    }
}