namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Text;
    using Utilities;

    // ShowEvent <eventName>
    internal class ShowEventCommand
    {
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            AuthenticationManager.Authorize();

            string eventName = inputArgs[0];

            if (!CommandHelper.IsEventExisting(eventName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.EventNotFound, eventName));
            }

            string eventDetails = this.ShowEvent(eventName);

            return eventDetails;
        }

        private string ShowEvent(string eventName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Event createdEvent = context.Events
                    .OrderByDescending(ev => ev.StartDate)
                    .FirstOrDefault(ev => ev.Name == eventName);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"[{createdEvent.Name}] [{createdEvent.StartDate}] [{createdEvent.EndDate}] [{createdEvent.Description}]");
                sb.AppendLine("Teams:");

                foreach (Team participatingTeam in createdEvent.ParticipatingTeams)
                {
                    sb.AppendLine($"--{participatingTeam.Name}");
                }

                return sb.ToString();
            }
        }
    }
}