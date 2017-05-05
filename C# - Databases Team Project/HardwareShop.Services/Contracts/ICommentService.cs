namespace HardwareShop.Services.Contracts
{
    using Models.ViewModels.Comments;

    public interface ICommentService
    {
        void AddComment(CommentViewModel model);
        void DeleteComment(int commentId);
        void EditComment(CommentViewModel model);
        DeleteCommentViewModel GetCommentForDelete(int commentId);
        CommentViewModel GetCommentViewModelById(int commentId);
    }
}