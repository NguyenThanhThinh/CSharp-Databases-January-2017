namespace PhotoShare.Client.Core.Commands
{
    using Services;
    using System;

    public class MakeFriendsCommand
    {
        private UserService userService;

        public MakeFriendsCommand(UserService userService)
        {
            this.userService = userService;
        }

        // MakeFriends <username1> <username2>
        public string Execute(string[] data)
        {
            if (!SecurityService.IsAuthenticated())
            {
                throw new InvalidOperationException("Login in order to add friends!");
            }

            string username = data[0];
            string username2 = data[1];

            if (SecurityService.GetCurrentUser().Username != username)
            {
                throw new InvalidOperationException("You can only add friends to yourself!");
            }

            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }
            else if (!this.userService.IsUserExisting(username2))
            {
                throw new ArgumentException($"User {username2} not found!");
            }

            if (this.userService.AreFriends(username, username2))
            {
                throw new InvalidOperationException($"{username2} is already friend with {username}!");
            }

            this.userService.MakeFriends(username, username2);

            return $"Friend {username2} added to {username}!";
        }
    }
}