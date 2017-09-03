namespace HardwareShop.Models.ViewModels.Reviews
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class DeleteReviewViewModel : ViewModelBase
    {
        public int ReviewId { get; set; }

        public int ItemId { get; set; }

        public string Content { get; set; }

        [Range(1.0, 5.0)]
        public double Score { get; set; }

        [DisplayName("Created on")]
        public DateTime ReviewDate { get; set; }

        public string AuthorId { get; set; }

        [DisplayName("Author")]
        public string AuthorUsername { get; set; }

        public bool IsAuthor()
        {
            return this.AuthorId == HttpContext.Current.User.Identity.GetUserId();
        }
    }
}