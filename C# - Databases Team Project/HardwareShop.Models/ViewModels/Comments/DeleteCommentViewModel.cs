namespace HardwareShop.Models.ViewModels.Comments
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.ComponentModel;
    using System.Web;

    public class DeleteCommentViewModel : ViewModelBase
    {
        public int CommentId { get; set; }

        public int ItemId { get; set; }

        public string Content { get; set; }

        [DisplayName("Created on")]
        public DateTime DatePosted { get; set; }

        public int ReviewId { get; set; }

        [DisplayName("Author")]
        public string AuthorUsername { get; set; }
        
        public string AuthorId { get; set; }

        public bool IsAuthor()
        {
            return this.AuthorId == HttpContext.Current.User.Identity.GetUserId();
        }
    }
}
