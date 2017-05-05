namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;
    using System.Linq;

    class DeclineInviteCommand
    {
        // DeclineInvite <teamName>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsInvitationExisting(teamName, AuthenticationManager.GetCurrentUser()))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvitationNotFound, teamName));
            }

            this.DeclineInvitation(teamName);

            return $"User {AuthenticationManager.GetCurrentUser().Username} declined invitation from team {teamName}!";
        }

        private void DeclineInvitation(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User loggedUser = AuthenticationManager.GetCurrentUser();
                Team teamToJoin = context.Teams.SingleOrDefault(team => team.Name == teamName);

                Invitation invitationToDecline = context.Invitations
                    .SingleOrDefault(invitation => invitation.TeamId == teamToJoin.Id &&
                                                   invitation.InvitedUserId == loggedUser.Id &&
                                                   invitation.IsActive);

                invitationToDecline.IsActive = false;
                context.SaveChanges();
            }
        }
    }
}
