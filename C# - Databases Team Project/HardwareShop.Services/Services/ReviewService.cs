namespace HardwareShop.Services.Services
{
    using Data;
    using System;
    using System.Web;
    using AutoMapper;
    using System.Linq;
    using Models.EntityModels;
    using Models.ViewModels.Items;
    using Models.ViewModels.Reviews;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using Contracts;
    using PagedList;

    public class ReviewService : IReviewService
    {
        public void AddReview(CreateReviewViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var review = Mapper.Map<Review>(model);
                review.ReviewDate = DateTime.Now;
                review.AuthorId = HttpContext.Current.User.Identity.GetUserId();

                context.Reviews.Add(review);
                context.SaveChanges();
            }
        }

        private Item GetItemById(int itemId, HardwareShopContext context)
        {
            return context.Items.FirstOrDefault(i => i.ItemId == itemId);
        }

        public void EditReview(EditReviewViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var review = this.GetReviewById(model.ReviewId, context);
                review.Content = model.Content;
                review.Score = model.Score;

                context.SaveChanges();
            }
        }

        public CreateReviewViewModel GetReviewViewModelById(int reviewId)
        {
            using (var context = new HardwareShopContext())
            {
                var review = this.GetReviewById(reviewId, context);

                if (review == null)
                {
                    return null;
                }

                var model = Mapper.Map<CreateReviewViewModel>(review);
                var items = context.Items;

                return model;
            }
        }

        public Review GetReviewById(int? reviewId, HardwareShopContext context)
        {
            return context.Reviews
                .FirstOrDefault(r => r.ReviewId == reviewId);
        }

        public void DeleteReview(int reviewId)
        {
            using (var context = new HardwareShopContext())
            {
                var review = GetReviewById(reviewId, context);
                review.IsDeleted = true;

                foreach (var comment in review.Comments)
                {
                    comment.IsDeleted = true;
                }

                context.SaveChanges();
            }
        }

        public DeleteReviewViewModel GetReviewForDelete(int reviewId)
        {
            using (var context = new HardwareShopContext())
            {
                var review = this.GetReviewById(reviewId, context);
                var model = Mapper.Map<DeleteReviewViewModel>(review);
                return model;
            }
        }

        public EditReviewViewModel GetReviewForEdit(int reviewId)
        {
            using (var context = new HardwareShopContext())
            {
                var review = this.GetReviewById(reviewId, context);
                var viewModel = Mapper.Map<EditReviewViewModel>(review);

                return viewModel;
            }
        }

        public bool IsItemExisting(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Items.Any(i => i.ItemId == itemId);
            }
        }

        public bool IsItemDeleted(int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Items.Any(i => i.ItemId == itemId && i.IsDeleted == false);
            }
        }

        public bool IsReviewDeleted(int reviewId)
        {
            using (var context = new HardwareShopContext())
            {
                return context.Reviews.Any(r => r.ReviewId == reviewId && r.IsDeleted == false);
            }
        }

        public IEnumerable<ListReviewsViewModel> GetReviewsByItemId(int? page, int itemId)
        {
            using (var context = new HardwareShopContext())
            {
                var reviews = context.Reviews.Where(r => r.ItemId == itemId && r.IsDeleted == false).ToList();
                var viewModel = Mapper.Map<IEnumerable<Review>, IEnumerable<ListReviewsViewModel>>(reviews);

                return viewModel.ToPagedList(page ?? 1, 3);
            }
        }
    }
}
