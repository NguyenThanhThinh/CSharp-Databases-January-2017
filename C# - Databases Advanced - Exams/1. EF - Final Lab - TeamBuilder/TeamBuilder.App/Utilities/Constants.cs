namespace TeamBuilder.App.Core.Utilities
{
    public static class Constants
    {
        public const int MinUsernameLength = 3;
        public const int MaxUsernameLength = 25;

        public const int MinPasswordLength = 6;
        public const int MaxPasswordLength = 30;

        public const int MaxFirstNameLength = 25;
        public const int MaxLastNameLength = 25;

        public const int MaxTeamNameLength = 25;
        public const int MaxTeamDescriptionLength = 32;

        public const string DateTimeFormat = "dd/MM/yyyy HH:mm";

        public static class ErrorMessages
        {
            // Common error messages
            public const string InvalidArgumentsCount = "Invalid arguments count!";

            public const string LoginFirst = "You should login first!";
            public const string LogoutFirst = "You should logout first!";

            public const string TeamOrUserDoesNotExist = "Team or user does not exist!";
            public const string InvitationIsAlreadySent = "Invitation is already sent!";

            public const string NotAllowed = "Not allowed!";

            public const string TeamNotFound = "Team {0} not found!";
            public const string UserNotFound = "User {0} not found!";
            public const string EventNotFound = "Event {0} not found!";
            public const string InvitationNotFound = "Invitation from {0} is not found!";

            public const string UserIsNotPartOfTeam = "User {0} is not a member in {1}!";

            public const string CommandNotAllowed = "Command not allowed! Use {0} instead!";
            public const string CannotAddSameTeamTwice = "Cannot add same team twice!";

            public const string StartDateAfterEndDate = "Start date time must be before end date time!";

            public const string FileNotFound = "Path {0} is not valid!";
            public const string InvalidXmlFormat = "Invalid Xml format!";

            // User error messages
            public const string UsernameNotValid = "Username {0} not valid!";

            public const string PasswordNotValid = "Password {0} not valid!";
            public const string PasswordsDoNotMatch = "Passwords do not match!";
            public const string FirstNameNotValid = "First name not valid!";
            public const string LastNameNotValid = "Last name not valid!";
            public const string AgeNotValid = "Age not valid!";
            public const string GenderNotValid = "Gender should be either “Male” or “Female”!";
            public const string UsernameIsAlreadyTaken = "Username {0} is already taken!";
            public const string UserOrPasswordIsInvalid = "Invalid username or password!";

            public const string InvalidDateFormat = "Please insert the dates in format: [dd/MM/yyyy HH:mm]!";

            // Team error messages
            public const string InvalidAcronym = "Acronym {0} not valid!";

            public const string TeamAlreadyExists = "Team {0} already exists!";
            public const string TeamNameOverflow = "Team name should be below 25 symbols!";
            public const string TeamDescriptionOverflow = "Team description should be below 32 symbols!";
        }
    }
}