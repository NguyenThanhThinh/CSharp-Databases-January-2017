namespace HardwareShop.Services.Services
{
    using AutoMapper;
    using Contracts;
    using Data;
    using Microsoft.AspNet.Identity;
    using Models.EntityModels;
    using Models.ViewModels.Comments;
    using System;
    using System.Linq;
    using System.Web;

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