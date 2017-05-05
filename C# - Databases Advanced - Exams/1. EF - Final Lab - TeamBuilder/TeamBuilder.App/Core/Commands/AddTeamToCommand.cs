namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;
    using System.Linq;
    
    // AddTeamTo <eventName> <teamName>
    class AddTeamToCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(2, inputArgs);

            AuthenticationManager.Authorize();

            string eventName = inputArgs[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            string teamName = inputArgs[1];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            if (!CommandHelper.IsUserCreatorOfEvent(eventName, AuthenticationManager.GetCurrentUser()))
            {
                throw new InvalidOperationException(Constants.ErrorMessages.NotAllowed);
            }

            this.AddTeamToEvent(teamName, eventName);

            return $"Team {teamName} was added to event {eventName}!";
        }

        private void AddTeamToEvent(string teamName, string eventName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team teamToAdd = context.Teams.SingleOrDefault(team => team.Name == teamName);
                Event createdEvent = context.Events
                         .OrderByDescending(ev => ev.StartDate)
                         .FirstOrDefault(ev => ev.Name == eventName);

                if (createdEvent.ParticipatingTeams.Any(team => team.Name == teamName))
                {
                    throw new InvalidOperationException(Constants.ErrorMessages.CannotAddSameTeamTwice);
                }

                createdEvent.ParticipatingTeams.Add(teamToAdd);
                context.SaveChanges();
            }
        }
    }
}
