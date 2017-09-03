using HardwareShop.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;

namespace HardwareShop.Models.ViewModels.Reviews
{
    public class ListReviewsViewModel
    {
        public int ReviewId { get; set; }

        public string Content { get; set; }

        public double Score { get; set; }

        public DateTime ReviewDate { get; set; }

        public int ItemId { get; set; }

        public string AuthorName { get; set; }

        public virtual ICollection<ListCommentsViewModel> Comments { get; set; }
    }
}