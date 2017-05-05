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

    public class ImportUsersCommand
    {
        // ImportUsers <filePath>
        public string Execute(string[] inputArgs)
        {
            Check.CheckLength(1, inputArgs);

            string filePath = inputArgs[0];

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format(Constants.ErrorMessages.FileNotFound, filePath));
            }

            List<User> users;

            try
            {
                users = GetUsersFromXmlFile(filePath);
            }
            catch (Exception)
            {
                throw new FormatException(Constants.ErrorMessages.InvalidXmlFormat);
            }

            this.AddUsers(users);

            return $"You have successfully imported {users.Count} users!";
        }

        private List<User> GetUsersFromXmlFile(string filePath)
        {
            XDocument documentXml = XDocument.Load(filePath);

            //IEnumerable<XElement> usersXml = documentXml.Root?.Elements();
            IEnumerable<XElement> usersXml = documentXml.XPathSelectElements("users/user");

            List<User> users = new List<User>();
            foreach (XElement userXml in usersXml)
            {
                string username = userXml.Element("username")?.Value;
                string password = userXml.Element("password")?.Value;
                string firstName = userXml.Element("first-name")?.Value;
                string lastName = userXml.Element("last-name")?.Value;
                int age = int.Parse(userXml.Element("age").Value);

                Gender gender = Gender.Male;
                bool isGenderFemaleValid = String.Equals(userXml.Element("gender")?.Value, 
                                                         Gender.Female.ToString(), 
                                                         StringComparison.InvariantCultureIgnoreCase);

                if (isGenderFemaleValid)
                {
                    gender = Gender.Female;
                }

                User user = new User()
                {
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age,
                    Gender = gender
                };

                users.Add(user);
            }

            return users;
        }

        private void AddUsers(List<User> users)
        {
            using (TeamBuilderContext context = new TeamBuilderContext())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
