namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using Utilities;

    public class DeleteCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(0, inputArgs);

            AuthenticationManager.Authorize();

            User user = AuthenticationManager.GetCurrentUser();

            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                context.Users.Attach(user);
                user.IsDeleted = true;
                context.SaveChanges();

                AuthenticationManager.Logout();
            }

            return $"User {user.Username} was deleted successfully!";
        }
    }
}
