using System;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class FindEmployeesByFirstNameStartingWith
    {
        public static void FindEmployeesByFirstName(SoftuniContext context)
        {
            string pattern = "Sa";
            var employees = context.Employees.Where(x => x.FirstName.StartsWith(pattern)).ToList();

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary})");
            }
        }
    }
}