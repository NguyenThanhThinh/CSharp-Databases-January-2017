namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using Utilities;

    class CreateTeamCommand
    {
        // CreateTeam <name> <acronym> <description>
        public string Execute(string[] inputArgs)
        {
            if (inputArgs.Length != 2 && inputArgs.Length != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(inputArgs));
            }

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (teamName.Length > Constants.MaxTeamNameLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamNameOverflow);
            }

            if (CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamAlreadyExists, teamName));
            }

            string acronym = inputArgs[1];

            if (acronym.Length != 3)
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.InvalidAcronym, acronym));
            }

            string description = inputArgs.Length == 3 ? inputArgs[2] : null;

            if (description != null && description.Length > Constants.MaxTeamDescriptionLength)
            {
                throw new ArgumentException(Constants.ErrorMessages.TeamDescriptionOverflow);
            }

            this.addTeam(teamName, acronym, description);

            return $"Team {teamName} was successfully created!";
        }

        private void addTeam(string teamName, string acronym, string description)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team team = new Team()
                {
                    Name = teamName,
                    Acronym = acronym, 
                    Description = description,
                    CreatorId = AuthenticationManager.GetCurrentUser().Id
                };

                User creator = AuthenticationManager.GetCurrentUser();

                context.Users.Attach(creator);
                creator.Teams.Add(team);
                // We need to attach the user to the database and then we can add the team to his teams or to add the user to the team
                //team.Members.Add(creator);

                context.Teams.Add(team);
                context.SaveChanges();
            }
        }
    }
}
