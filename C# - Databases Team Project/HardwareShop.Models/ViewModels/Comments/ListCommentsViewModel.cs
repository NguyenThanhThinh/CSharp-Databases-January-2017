using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
