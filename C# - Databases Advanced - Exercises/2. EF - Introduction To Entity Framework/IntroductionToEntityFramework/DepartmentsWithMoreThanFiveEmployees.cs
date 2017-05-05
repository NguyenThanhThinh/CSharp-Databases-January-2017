using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class DepartmentsWithMoreThanFiveEmployees
    {
        public static void GetDepartmentsWithMoreThanFiveEmployees(SoftuniContext context)
        {
            var departments = context.Departments.Where(x => x.Employees.Count > 5)
                                                 .OrderBy(x => x.Employees.Count);

            foreach (var department in departments)
            {
                Console.WriteLine($"{department.Name} {department.Manager.FirstName}");
                foreach (var employee in department.Employees)
                {
                    Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.JobTitle}");
                }
            }
        }
    }
}
