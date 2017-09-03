using System;
using System.Data.Entity;
using System.Linq;
using TeamBuilder.App.Core.Utilities;
using TeamBuilder.Data;
using TeamBuilder.Models;

namespace TeamBuilder.App.Core.Commands
{
    internal class InviteToTeamCommand
    {
        // InviteToTeam <teamName> <username>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];
            string username = inputArgs[1];

            if (!CommandHelper.IsTeamExisting(teamName) || !CommandHelper.IsUserExisting(username))
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamOrUserDoesNotExist);
            }

            if (this.IsInvitationPending(teamName, username))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.InvitationIsAlreadySent);
            }

            if (!this.IsCreatorOrMemberOfTeam(teamName))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            this.SendInvite(teamName, username);

            return $"Team {teamName} invited {username}!";
        }

        private bool IsInvitationPending(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Invitations
                    .Include(invitation => invitation.Team)
                    .Include(invitation => invitation.InvitedUser)
                    .Any(invitation => invitation.Team.Name == teamName &&
                                       invitation.InvitedUser.Username == username &&
                                       invitation.IsActive);
            }
        }

        private bool IsCreatorOrMemberOfTeam(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                User loggedUser = AuthenticationManager.GetCurrentUser();

                return context.Teams
                    .Include(team => team.Members)
                    .Any(team => team.Name == teamName &&
                              (team.CreatorId == loggedUser.Id || team.Members.Any(member => member.Username == loggedUser.Username)));
            }
        }

        private void SendInvite(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team teamSendingInvitation = context.Teams.SingleOrDefault(team => team.Name == teamName);
                User userReceivingInvitation = context.Users.SingleOrDefault(user => user.Username == username);

                Invitation invitation = new Invitation()
                {
                    InvitedUser = userReceivingInvitation,
                    Team = teamSendingInvitation
                };

                context.Invitations.Add(invitation);
                context.SaveChanges();
            }
        }
    }
}