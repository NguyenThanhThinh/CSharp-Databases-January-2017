using HardwareShop.Models.EntityModels;
using HardwareShop.Models.ViewModels.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
