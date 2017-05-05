namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;
    using System.Globalization;

    public class CreateEventCommand
    {
        // CreateEvent <name> <description> <startDate> <endDate>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(6, inputArgs);

            AuthenticationManager.Authorize();

            string eventName = inputArgs[0];
            string description = inputArgs[1];

            DateTime startDateTime;

            bool isStartDateTime = DateTime.TryParseExact(inputArgs[2] + " " + inputArgs[3],
                                             Constants.DateTimeFormat, CultureInfo.InvariantCulture,
                                             DateTimeStyles.None, out startDateTime);

            DateTime endDateTime;

            bool isEndDateTime = DateTime.TryParseExact(inputArgs[4] + " " + inputArgs[5],
                                             Constants.DateTimeFormat, CultureInfo.InvariantCulture,
                                             DateTimeStyles.None, out endDateTime);

            if (!isStartDateTime || !isEndDateTime)
            {
                throw new ArgumentException(Constants.ErrorMessages.InvalidDateFormat);
            }

            if (startDateTime > endDateTime)
            {
                throw new ArgumentException(Constants.ErrorMessages.StartDateAfterEndDate);
            }

            this.CreateEvent(eventName, description, startDateTime, endDateTime);

            return $"Event {eventName} was created successfully!";
        }

        private void CreateEvent(string eventName, string description, DateTime startDateTime, DateTime endDateTime)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Event createdEvent = new Event()
                {
                    Name = eventName,
                    Description = description,
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    CreatorId = AuthenticationManager.GetCurrentUser().Id
                };

                context.Events.Add(createdEvent);
                context.SaveChanges();
            }
        }
    }
}
