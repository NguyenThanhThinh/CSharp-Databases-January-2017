namespace HardwareShop.Services.Services
{
    using Data;
    using System;
    using AutoMapper;
    using System.Web;
    using System.Linq;
    using Models.EntityModels;
    using Microsoft.AspNet.Identity;
    using Models.ViewModels.Comments;
    using Contracts;

    public class CommentService : ICommentService
    {
        public void AddComment(CommentViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var comment = Mapper.Map<Comment>(model);
                comment.DatePosted = DateTime.Now;
                comment.AuthorId = HttpContext.Current.User.Identity.GetUserId();

                context.Comments.Add(comment);
                context.SaveChanges();
            }
        }

        public void EditComment(CommentViewModel model)
        {
            using (var context = new HardwareShopContext())
            {
                var comment = this.GetCommentById(model.CommentId, context);
                comment.Content = model.Content;

                context.SaveChanges();
            }
        }

        public CommentViewModel GetCommentViewModelById(int commentId)
        {
            using (var context = new HardwareShopContext())
            {
                var comment = this.GetCommentById(commentId, context);
                var model = Mapper.Map<CommentViewModel>(comment);

                return model;
            }
        }

        private Comment GetCommentById(int commentId, HardwareShopContext context)
        {
            return context.Comments
               .FirstOrDefault(c => c.CommentId == commentId);
        }

        public void DeleteComment(int commentId)
        {
            using (var context = new HardwareShopContext())
            {
                var comment = GetCommentById(commentId, context);
                comment.IsDeleted = true;

                context.SaveChanges();
            }
        }

        public DeleteCommentViewModel GetCommentForDelete(int commentId)
        {
            using (var context = new HardwareShopContext())
            {
                var comment = this.GetCommentById(commentId, context);
                var model = Mapper.Map<DeleteCommentViewModel>(comment);

                return model;
            }
        }
    }
}