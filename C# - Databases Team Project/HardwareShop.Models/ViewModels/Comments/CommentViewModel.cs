namespace HardwareShop.Models.ViewModels.Comments
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class CommentViewModel : ViewModelBase
    {
        public int CommentId { get; set; }

        public int ItemId { get; set; }

        [Required]
        public string Content { get; set; }

        public int ReviewId { get; set; }

        [DisplayName("Created on")]
        public DateTime DatePosted { get; set; }

        public string AuthorId { get; set; }

        public bool IsAuthor()
        {
            return this.AuthorId == HttpContext.Current.User.Identity.GetUserId();
        }
    }
}