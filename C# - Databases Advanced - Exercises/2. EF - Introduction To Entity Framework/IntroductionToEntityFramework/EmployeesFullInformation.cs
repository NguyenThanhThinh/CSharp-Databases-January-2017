using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class EmployeesFullInformation
    {
        public static void GetEmployeesFullInformation(SoftuniContext context)
        {
            var employees = context.Employees
                                   .Select(x => new { x.EmployeeID, x.FirstName, x.MiddleName, x.LastName, x.JobTitle, x.Salary })
                                   .OrderBy(x => x.EmployeeID);

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary}");
            }
        }
    }
}
