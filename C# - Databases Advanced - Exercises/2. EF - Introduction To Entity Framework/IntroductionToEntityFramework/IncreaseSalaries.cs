using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class IncreaseSalaries
    {
        public static void IncreaseSalariesMethod(SoftuniContext context)
        {
            var employees = context.Employees.Where(x => x.Department.Name == "Engineering" 
                                                      || x.Department.Name == "Tool Design"
                                                      || x.Department.Name == "Marketing" 
                                                      || x.Department.Name == "Information Services");

            foreach (var employee in employees)
            {
                employee.Salary *= (decimal)1.12;
            }

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} (${employee.Salary})");
            }

            context.SaveChanges();
        }
    }
}
