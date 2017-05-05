namespace PhotoShare.Client.Core.Commands
{
    using System;
    using Services;
    using System.Text;
    using System.Collections.Generic;

    public class ListFriendsCommand
    {
        private UserService userService;

        public ListFriendsCommand(UserService userService)
        {
            this.userService = userService;
        }

        // PrintFriendsList <username>
        public string Execute(string[] data)
        {
            string username = data[0];

            if (!this.userService.IsUserExisting(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            List<string> friends = this.userService.GetUserFriends(username);

            if (friends.Count == 0)
            {
                return $"No friends for this user!";
            }

            return $"Friends of {username}:\n-{String.Join("\n-", friends)}";

            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("Friends of {username}:");
            //foreach (string friend in friends)
            //{
            //    sb.AppendLine($"-{friend}");
            //}

            //return sb.ToString().Trim();
        }
    }
}
