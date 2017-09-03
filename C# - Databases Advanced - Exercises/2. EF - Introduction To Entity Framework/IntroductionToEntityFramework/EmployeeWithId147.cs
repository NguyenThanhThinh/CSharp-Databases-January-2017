using System;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class EmployeeWithId147
    {
        public static void GetEmployeeWithId147(SoftuniContext context)
        {
            var employee = context.Employees.First(x => x.EmployeeID == 147);
            //var employee = context.Employees.Find(147);

            Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle}");

            foreach (var project in employee.Projects.OrderBy(x => x.Name))
            {
                Console.WriteLine(project.Name);
            }
        }
    }
}