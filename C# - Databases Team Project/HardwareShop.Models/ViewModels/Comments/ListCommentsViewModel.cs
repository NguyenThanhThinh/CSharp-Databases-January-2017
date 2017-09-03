using System;

namespace HardwareShop.Models.ViewModels.Comments
{
    public class ListCommentsViewModel
    {
        public int CommentId { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public DateTime DatePosted { get; set; }
    }
}