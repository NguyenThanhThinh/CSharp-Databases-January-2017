namespace TeamBuilder.App.Core.Commands
{
    using Data;
    using Models;
    using System;
    using System.IO;
    using Utilities;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.Collections.Generic;

    class ImportTeamsCommand
    {
        // ImportTeams <filePath>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string filePath = inputArgs[0];

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format(Constants.ErrorMessages.FileNotFound, filePath));
            }

            List<Team> teams;

            try
            {
                teams = GetTeamsFromXmlFile(filePath);
            }
            catch (Exception)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidXmlFormat);
            }

            this.AddTeams(teams);

            return $"You have successfully imported {teams.Count} teams!";
        }

        private List<Team> GetTeamsFromXmlFile(string filePath)
        {
            XDocument documentXml = XDocument.Load(filePath);

            IEnumerable<XElement> teamsXml = documentXml.Root?.Elements();
            //IEnumerable<XElement> teamsXml = documentXml.XPathSelectElements("teams/team");

            List<Team> teams = new List<Team>();
            foreach (XElement teamXml in teamsXml)
            {
                string teamName = teamXml.Element("name")?.Value;
                string acronym = teamXml.Element("acronym")?.Value;
                string description = teamXml.Element("description")?.Value;
                int creatorId = int.Parse(teamXml.Element("creator-id").Value);
                
                Team team = new Team()
                {
                    Name = teamName,
                    Acronym = acronym,
                    Description = description,
                    CreatorId = creatorId
                };

                teams.Add(team);
            }

            return teams;
        }

        private void AddTeams(List<Team> teams)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                context.Teams.AddRange(teams);
                context.SaveChanges();
            }
        }
    }
}
