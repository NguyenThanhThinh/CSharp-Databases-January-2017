namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;
    using System.Linq;

    class KickMemberCommand
    {
        // KickMember <teamName> <username>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            string username = inputArgs[1];

            if (!CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserNotFound, username));
            }
            
            if (!CommandHelper.IsMemberOfTeam(teamName, username))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.UserIsNotPartOfTeam, username, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, AuthenticationManager.GetCurrentUser()))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            if (AuthenticationManager.GetCurrentUser().Username == username)
            {
                // User tries to kick himself
                throw new InvalidOperationException(string.Format(Constants.ErrorMessages.CommandNotAllowed, "DisbandTeam"));
            }

            this.KickUserFromTeam(teamName, username);

            return $"User {username} was kicked from {teamName}!";
        }

        private void KickUserFromTeam(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User userToKick = context.Users.SingleOrDefault(user => user.Username == username);
                Team teamOfUserToKick = context.Teams.SingleOrDefault(team => team.Name == teamName);

                teamOfUserToKick.Members.Remove(userToKick);

                context.SaveChanges();
            }
        }
    }
}
