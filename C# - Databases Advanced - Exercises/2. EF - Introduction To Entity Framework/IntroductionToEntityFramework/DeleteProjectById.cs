using System;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class DeleteProjectById
    {
        public static void DeleteProjectWithId(SoftuniContext context)
        {
            var project = context.Projects.Find(2);

            var employees = project.Employees;

            foreach (var employee in employees)
            {
                employee.Projects.Remove(project);
            }

            context.Projects.Remove(project);
            context.SaveChanges();

            var projects = context.Projects.Select(x => x.Name).Take(10);

            foreach (var proj in projects)
            {
                Console.WriteLine(proj);
            }
        }
    }
}