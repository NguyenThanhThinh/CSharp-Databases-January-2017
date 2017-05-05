namespace HardwareShop.Models.EntityModels
{
    using System;

    public class Comment
    {
        public Comment()
        {
            this.DatePosted = DateTime.Now;
        }

        public int CommentId { get; set; }

        public string Content { get; set; }

        public DateTime DatePosted { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int ReviewId { get; set; }

        public virtual Review Review { get; set; }

        public bool IsDeleted { get; set; }
    }
}