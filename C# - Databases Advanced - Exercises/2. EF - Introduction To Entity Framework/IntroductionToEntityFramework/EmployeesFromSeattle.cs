using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class EmployeesFromSeattle
    {
        public static void GetEmployeesFromSeattle(SoftuniContext context)
        {
            var employees = context.Employees
                                   .Where(x => x.Department.Name == "Research and Development")
                                   .Select(x => new { x.FirstName, x.LastName, x.Department, x.Salary })
                                   .OrderBy(x => x.Salary)
                                   .ThenByDescending(x => x.FirstName);

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} from Research and Development - ${employee.Salary:F2}");
            }
        }
    }
}
