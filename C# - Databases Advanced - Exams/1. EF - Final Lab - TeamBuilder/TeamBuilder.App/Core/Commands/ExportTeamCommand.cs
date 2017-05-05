namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using System.IO;
    using Utilities;
    using System.Linq;
    using Newtonsoft.Json;
    using System.Data.Entity;

    class ExportTeamCommand
    {
        // ExportTeam <teamName>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string teamName = inputArgs[0];

            if (!CommandHelper.IsTeamExisting(teamName))
            {
                throw new ArgumentException(string.Format(Constants.ErrorMessages.TeamNotFound, teamName));
            }

            Team team = this.GetTeamByNameWithMembers(teamName);

            this.ExportTeam(team);

            return $"Team {teamName} was exported successfully!";
        }

        private Team GetTeamByNameWithMembers(string teamName)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                //return context.Teams
                //    .Include(team => team.Members)
                //    .SingleOrDefault(team => team.Name == teamName);

                var teamToExport = context.Teams
                   .SingleOrDefault(team => team.Name == teamName);

                return teamToExport;
            }
        }

        private void ExportTeam(Team team)
        {
            string json = JsonConvert.SerializeObject(new
            {
                Name = team.Name,
                Acronym = team.Acronym,
                Members = team.Members.Select(member => member.Username)
            }, Formatting.Indented);

            File.WriteAllText("team.json", json);
        }
    }
}
