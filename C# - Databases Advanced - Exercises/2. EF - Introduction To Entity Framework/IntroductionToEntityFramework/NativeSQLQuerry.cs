using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class NativeSQLQuerry
    {
        public static void UseNativeSQLQuerry(SoftuniContext context)
        {
            var timer = new Stopwatch();

            timer.Start();
            PrintNamesWithLinq(context);
            timer.Stop();
            Console.WriteLine($"Linq: {timer.Elapsed}");
            timer.Reset();

            timer.Start();
            PrintNamesWithNativeQuery(context);
            timer.Stop();
            Console.WriteLine($"Native: {timer.Elapsed}");
            timer.Reset();
        }

        private static void PrintNamesWithLinq(SoftuniContext context)
        {
            var employeesNames = context.Employees.Where(x => x.Projects.Count(p => p.StartDate.Year == 2002) != 0)
                                                  .Select(x => x.FirstName)
                                                  .GroupBy(s => s);
            foreach (var name in employeesNames)
            {

            }
        }

        private static void PrintNamesWithNativeQuery(SoftuniContext context)
        {
            string query = "SELECT e.FirstName FROM Employees AS e " +
                             "JOIN EmployeesProjects AS ep " +
                               "ON e.EmployeeId = ep.EmployeeId " +
                             "JOIN Projects AS p " +
                               "ON ep.ProjectId = p.ProjectId " +
                              "AND YEAR(p.StartDate) = 2002 " +
                            "GROUP BY e.FirstName";
            var result = context.Database.SqlQuery<string>(query);
            foreach (var res in result)
            {

            }
        }
    }
}
