namespace WeddingsPlanner.Import
{
    using Data;
    using DTOs;
    using Models;
    using System;
    using System.IO;
    using Utilities;
    using System.Linq;
    using Models.Enums;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Text.RegularExpressions;

    public class JsonMethods
    {
        private static IEnumerable<T> ParseJson<T>(string filePath)
        {
            string json = File.ReadAllText(filePath);
            IEnumerable<T> dtos = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return dtos;
        }

        public static void ImportAgencies()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                IEnumerable<AgencyDto> agenciesListDto = ParseJson<AgencyDto>(Constants.AgenciesPath);

                foreach (AgencyDto agencyDto in agenciesListDto)
                {
                    Agency agencyEntity = new Agency()
                    {
                        Name = agencyDto.Name,
                        EmployeesCount = agencyDto.EmployeesCount,
                        Town = agencyDto.Town
                    };

                    context.Agencies.Add(agencyEntity);
                    context.SaveChanges();
                    Console.WriteLine($"Successfully imported {agencyEntity.Name}!");
                }
            }
        }

        public static void ImportPeople()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                IEnumerable<PersonDto> personsListDto = ParseJson<PersonDto>(Constants.PeoplePath);

                List<Person> people = new List<Person>();

                foreach (PersonDto personDto in personsListDto)
                {
                    // We can make the checks for FirstName, MiddleName LastName Length and Email Pattern here,  
                    // add everything to a List<Person> and save it in the Database with context.SaveChanges() and no errors

                    // OR

                    // In order to use all the attribute validations which we have implemented in the models,
                    // we have to use a try-catch construction with Context.SaveChanges so can the attributes to work as expected;

                    if (personDto.FirstName == null || personDto.FirstName.Length > 60)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    if (personDto.MiddleInitial == null || personDto.MiddleInitial.Length != 1)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    if (personDto.LastName == null || personDto.LastName.Length < 2)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    Regex regex = new Regex(@"^[a-zA-Z0-9]+@[a-z]{1,}.[a-z]{1,}$");
                    if (personDto.Email != null && !regex.IsMatch(personDto.Email))
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }

                    Gender gender;
                    bool isGenderValid = Enum.TryParse(personDto.Gender.ToString(), out gender);

                    if (!isGenderValid)
                    {
                        gender = Gender.NotSpecified;
                    }

                    Person personEntity = new Person()
                    {
                        FirstName = personDto.FirstName,
                        MiddleNameInitial = personDto.MiddleInitial,
                        LastName = personDto.LastName,
                        Gender = gender,
                        BirthDate = personDto.Birthday,
                        Phone = personDto.Phone,
                        Email = personDto.Email
                    };

                    people.Add(personEntity);
                    Console.WriteLine($"Successfully imported {personEntity.FullName}!");
                }

                context.People.AddRange(people);
                context.SaveChanges();

                // We have to use this syntax in order all the models attributes to work as expected: 
                //    
                //try
                //{
                //    context.People.Add(personEntity);
                //    context.SaveChanges();
                //    Console.WriteLine($"Successfully imported {personEntity.FullName}!");
                //}
                //catch (DbEntityValidationException)
                //{
                //    context.People.Remove(personEntity);
                //    Console.WriteLine(Messages.InvalidData);
                //}
            }
        }

        public static void ImportWeddings()
        {
            using (WeddingsPlannerContext context = new WeddingsPlannerContext())
            {
                IEnumerable<WeddingDto> weddingsListDto = ParseJson<WeddingDto>(Constants.WeddingsPath);

                foreach (WeddingDto weddingDto in weddingsListDto)
                {
                    Person bride = context.People.FirstOrDefault(person =>
                        person.FirstName + " " + person.MiddleNameInitial + " " + person.LastName == weddingDto.Bride);
                    Person bridesgroom = context.People.FirstOrDefault(person =>
                        person.FirstName + " " + person.MiddleNameInitial + " " + person.LastName == weddingDto.Bridegroom);
                    Agency agency = context.Agencies.FirstOrDefault(ag => ag.Name == weddingDto.Agency);

                    if (bride == null || bridesgroom == null || weddingDto.Date == null || agency == null)
                    {
                        Console.WriteLine(Messages.InvalidData);
                        continue;
                    }
                    
                    Wedding weddingEntity = new Wedding()
                    {
                        Bride = bride,
                        Bridegroom = bridesgroom,
                        Date = weddingDto.Date.Value,
                        Agency = agency
                    };
                        
                    foreach (GuestDto guestDto in weddingDto.Guests)
                    {
                        Person guest = context.People.FirstOrDefault(person =>
                            person.FirstName + " " + person.MiddleNameInitial + " " + person.LastName == guestDto.Name);

                        Family family;
                        bool isGenderValid = Enum.TryParse(guestDto.Family.ToString(), out family);

                        if (!isGenderValid)
                        {
                            Console.WriteLine(Messages.InvalidData);
                            continue;
                        }

                        if (guest != null)
                        {
                            Invitation invitation = new Invitation()
                            {
                                Guest = guest,
                                IsAttending = guestDto.RSVP,
                                Family = family
                            };

                            weddingEntity.Invitations.Add(invitation);
                        }
                    }

                    try
                    {
                        context.Weddings.Add(weddingEntity);
                        context.SaveChanges();
                        Console.WriteLine($"Successfully imported wedding of {bride.FirstName} and {bridesgroom.FirstName}");
                    }
                    catch (DbEntityValidationException)
                    {
                        context.Weddings.Remove(weddingEntity);
                        Console.WriteLine(Messages.InvalidData);
                    }
                }
            }
        }
    }
}
