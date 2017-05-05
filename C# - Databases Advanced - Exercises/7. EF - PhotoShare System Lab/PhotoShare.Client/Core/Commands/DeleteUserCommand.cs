namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Services;

    public class DeleteUserCommand
    {
        private UserService userService;

        public DeleteUserCommand(UserService userService)
        {
            this.userService = userService;
        }

        // DeleteUser <username>
        public string Execute(string[] data)
        {
            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to delete your profile!");
            }

            string username = data[0];

            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (SecurityService.GetCurrentUser().Username != username)
            {
                throw new InvalidOperationException("You can delete only your profile!");
            }

            this.userService.Delete(username);

            return $"User {username} was deleted successfully!";

        }
    }
}
