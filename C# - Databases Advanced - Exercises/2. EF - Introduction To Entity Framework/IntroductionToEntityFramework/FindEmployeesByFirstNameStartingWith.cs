using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class FindEmployeesByFirstNameStartingWith
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
