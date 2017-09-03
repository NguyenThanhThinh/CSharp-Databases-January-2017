using System;
using System.Globalization;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class FindEmployeesInPeriod
    {
        public static void FindingEmployeesInPeriod(SoftuniContext context)
        {
            var employees = context.Employees.Where(x => x.Projects.Count(p => p.StartDate.Year >= 2001 && p.StartDate.Year <= 2003) > 0)
                                             .Take(30);

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.Manager.FirstName}");
                foreach (var project in employee.Projects)
                {
                    string dateTimeFormat = "M/d/yyyy h:mm:ss tt";
                    Console.WriteLine($"--{project.Name} {project.StartDate.ToString(dateTimeFormat, CultureInfo.InvariantCulture)} {project.EndDate:M/d/yyyy h:mm:ss tt}");
                    //the result on the console depends on the regional options in the control panel, because
                    //the project.EndDate can be Null and doesn't support .ToString();
                    //for correct result in Judge the regional options for "Date formats - Short date" should be "M/d/yyyy"
                }
            }
        }
    }
}