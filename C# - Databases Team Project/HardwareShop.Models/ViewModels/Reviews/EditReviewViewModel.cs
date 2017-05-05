namespace HardwareShop.Models.ViewModels.Reviews
{
    using Microsoft.AspNet.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class EditReviewViewModel : ViewModelBase
    {
        public int ReviewId { get; set; }

        public int ItemId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1.0, 5.0)]
        public double Score { get; set; }

        public string AuthorId { get; set; }

        public bool IsAuthor()
        {
            return this.AuthorId == HttpContext.Current.User.Identity.GetUserId();
        }
    }
}
