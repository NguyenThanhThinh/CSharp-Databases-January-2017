namespace Softuni.Client
{
    using SoftUni.Data;
    using System;
    using System.Linq;

    internal class SoftUniMain
    {
        private static void Main()
        {
            SoftUniContext context = new SoftUniContext();

            // Task 17
            //CallStoredProcedure(context);

            //Task 18
            //EmployeesMaximumSalaries(context);
        }

        private static void CallStoredProcedure(SoftUniContext context)
        {
            // Solution I - the context has a method which returns the result;
            var projectsInfo = context.FindAllProjectsForGivenEmployee("Ruth", "Ellerbrock");

            // Solution II - we can directly run the procedure using SQL query with parameters;
            //var projectsInfo = context.Database.SqlQuery<ProjectInfo>("EXEC FindAllProjectsForGivenEmployee 'Ruth', 'Ellerbrock'");

            foreach (ProjectInfo projectInfo in projectsInfo)
            {
                Console.WriteLine($"{projectInfo.Name} - {projectInfo.Description.Substring(0, 20)}... {projectInfo.StartDate}");
            }
        }

        private static void EmployeesMaximumSalaries(SoftUniContext context)
        {
            var departments = context.Departments
                .GroupBy(department => new
                {
                    DepartmentName = department.Name,
                    DepartmentMaxSalary = department.Employees.Max(e => e.Salary)
                })
                .Where(group => group.Key.DepartmentMaxSalary < 30000 || group.Key.DepartmentMaxSalary > 70000)
                .ToList();

            foreach (var department in departments)
            {
                Console.WriteLine($"{department.Key.DepartmentName} - {department.Key.DepartmentMaxSalary}");
            }
        }
    }
}