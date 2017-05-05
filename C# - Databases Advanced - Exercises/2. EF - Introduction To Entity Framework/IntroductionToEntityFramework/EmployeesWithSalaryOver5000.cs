using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class EmployeesWithSalaryOver5000
    {
        public static void GetEmployeesWithSalariesOver5000(SoftuniContext context)
        {
            var employeesNames = context.Employees
                                        .Where(x => x.Salary > 50000)
                                        .Select(x => x.FirstName);

            foreach (var employeeName in employeesNames)
            {
                Console.WriteLine(employeeName);
            }
        }
    }
}
