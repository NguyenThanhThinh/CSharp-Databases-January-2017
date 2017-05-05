namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;
    using System.Linq;
    using System.Collections.Generic;

    class DisbandTeamCommand
    {
        // Disband <teamName>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            User loggedUser = AuthenticationManager.GetCurrentUser();

            if (!CommandHelper.IsUserCreatorOfTeam(teamName, loggedUser))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            this.DisbandTeam(teamName);

            return $"{teamName} was disbanded!";
        }

        private void DisbandTeam(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team teamToRemove = context.Teams.SingleOrDefault(team => team.Name == teamName);
                List<Invitation> invitationsToRemove = context.Invitations
                    .Where(invitation => invitation.TeamId == teamToRemove.Id)
                    .ToList();

                context.Invitations.RemoveRange(invitationsToRemove);
                context.Teams.Remove(teamToRemove);
                context.SaveChanges();
            }
        }
    }
}
