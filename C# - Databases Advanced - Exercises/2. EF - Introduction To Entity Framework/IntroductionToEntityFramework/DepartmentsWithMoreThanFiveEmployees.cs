using System;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class DepartmentsWithMoreThanFiveEmployees
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