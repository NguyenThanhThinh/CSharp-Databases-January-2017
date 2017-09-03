namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Text;
    using Utilities;

    internal class ShowTeamCommand
    {
        // ShowTeam <teamName>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            AuthenticationManager.Authorize();

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            string teamDetails = this.ShowTeam(teamName);

            return teamDetails;
        }

        private string ShowTeam(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                Team teamToShow = context.Teams.SingleOrDefault(team => team.Name == teamName);

                StringBuilder sb = new StringBuilder();

                sb.AppendLine($"[{teamToShow.Name}] [{teamToShow.Acronym}]");
                sb.AppendLine("Members:");

                foreach (User member in teamToShow.Members)
                {
                    sb.AppendLine($"--{member.Username}");
                }

                return sb.ToString();
            }
        }
    }
}