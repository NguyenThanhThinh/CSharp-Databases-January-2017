namespace HardwareShop.Models.EntityModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        public Review()
        {
            this.ReviewDate = DateTime.Now;
            this.Comments = new HashSet<Comment>();
        }

        public int ReviewId { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int ItemId { get; set; }

        public virtual Item Item { get; set; }

        public string Content { get; set; }

        [Range(1, 5)]
        public double Score { get; set; }

        public DateTime ReviewDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}