namespace TeamBuilder.App.Core.Utilities
{
    using Data;
    using Models;
    using System.Linq;

    class CommandHelper
    {
        public static bool IsTeamExisting(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Teams.Any(team => team.Name == teamName);
            }
        }

        public static bool IsUserExisting(string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Users.Any(user =>
                        user.Username == username &&
                        user.IsDeleted == false);
            }
        }

        public static bool IsEventExisting(string eventName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Events.Any(createdEvent => createdEvent.Name == eventName);
            }
        }

        public static bool IsInvitationExisting(string teamName, User user)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Invitations.Any(invitation =>
                                invitation.Team.Name == teamName &&
                                invitation.InvitedUserId == user.Id &&
                                invitation.IsActive);
            }
        }

        public static bool IsUserCreatorOfTeam(string teamName, User user)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Teams.Any(team =>
                                team.Name == teamName &&
                                team.Creator.Id == user.Id);
            }
        }

        public static bool IsUserCreatorOfEvent(string eventName, User user)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Events.Any(createdEvent =>
                                createdEvent.Name == eventName &&
                                createdEvent.CreatorId == user.Id);
            }
        }

        public static bool IsMemberOfTeam(string teamName, string username)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                return context.Teams.Any(team =>
                                team.Name == teamName &&
                                team.Members.Any(member => member.Username == username));
            }
        }
    }
}
