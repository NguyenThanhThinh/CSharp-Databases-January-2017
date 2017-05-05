namespace Performance.Client
{
    using Data;
    using Models;
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Data.Entity;

    class Application
    {
        static void Main(string[] args)
        {
            EmployeeContext context = new EmployeeContext();
            Stopwatch stopwatch = new Stopwatch();
            long timePassed = 0L;
            int testCount = 10; // Amount of tests to perform
            for (int i = 0; i < testCount; i++)
            {
                // Clear all query cache
                context.Database.ExecuteSqlCommand("CHECKPOINT; DBCC DROPCLEANBUFFERS;");
                stopwatch.Start();

                // Task 5. / Task 7. 
                // Try Eager Loading Performance without Select:
                // Eager Loading is faster - the SQL query contains Join statements and all the columns information:
                //QueryWithEagerLoading(context);

                // Task 5. / Task 7. 
                // Try Lazy Loading Performance without Select:
                // Lazy loading is slower - te SQL query contains Foreign Key Id's instead of Join statements and all the columns information:
                //QueryWithLazyLoading(context);


                // Task 6. Try Eager Loading Performance with Select:
                // The SQL queries are similar with Join statements, the performance is also similar:
                //SelectQueryWithEagerLoading(context);

                // Task 6. Try Lazy Loading Performance with Select:
                // The SQL queries are similar with Join statements, the performance is also similar:
                //SelectQueryWithLazyLoading(context);


                // Task 8. For more complicated Where queries => the SQL query with Lazy Loading is easier and the performance is faster:
                //SelectQueryWithEagerAndLazyLoading(context);


                // Task 9. ToList() is first and OrderBy() is second - the SQL query is NOT using Join statement and the performance is faster:
                // Task 9. OrderBy() is first and ToList() is second - the SQL query is using Join statement and the performance is slower:
                //UsingOrderByAndToList(context);


                // Task 10. The Where query should be first and ToList() should be after it for better performance: 
                QueryOpitmizing(context);

                stopwatch.Stop();
                timePassed += stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
            }

            TimeSpan averageTimePassed = TimeSpan.FromMilliseconds(timePassed / (double)testCount);
            Console.WriteLine(averageTimePassed);
        }

        // Eager Loading is faster - the SQL query contains Join statements and all the columns information:
        private static void QueryWithEagerLoading(EmployeeContext context)
        {
            List<Employee> employees = context.Employees.Where(e => e.Salary < 3000).Include(e => e.Department).Include(e => e.Address).ToList();

            foreach (Employee employee in employees)
            {
                string result = $"{employee.FirstName} - {employee.Department.Name} - {employee.Address.AddressText}";
            }
        }

        // Lazy loading is slower - te SQL query contains Foreign Key Id's instead of Join statements and all the columns information:
        private static void QueryWithLazyLoading(EmployeeContext context)
        {
            List<Employee> employees = context.Employees.Where(e => e.Salary < 3000).ToList();

            foreach (Employee employee in employees)
            {
                string result = $"{employee.FirstName} - {employee.Department.Name} - {employee.Address.AddressText}";
            }
        }

        // The SQL queries are similar with Join statements, the performance is also similar:
        private static void SelectQueryWithEagerLoading(EmployeeContext context)
        {
            var employees = context.Employees.Include("Department").Include("Address").Select(e => new
                {
                    e.FirstName,
                    e.Department.Name,
                    e.Address.AddressText
                }).ToList();

            foreach (var employee in employees)
            {
                string result = $"{employee.FirstName} - {employee.Name} - {employee.AddressText}";
            }
        }

        // The SQL queries are similar with Join statements, the performance is also similar:
        private static void SelectQueryWithLazyLoading(EmployeeContext context)
        {
            var employees = context.Employees.Select(e => new
            {
                e.FirstName,
                e.Department.Name,
                e.Address.AddressText

            }).ToList();

            foreach (var employee in employees)
            {
                string result = $"{employee.FirstName} - {employee.Name} - {employee.AddressText}";
            }
        }

        // For more complicated Where queries => the SQL query with Lazy Loading is easier and the performance is faster:
        private static void SelectQueryWithEagerAndLazyLoading(EmployeeContext context)
        {
            var employeesEager = context.Employees.Include(e => e.Department).Include(e => e.EmployeesProjects)
                    .Where(e => e.EmployeesProjects.Count(ep => ep.EmployeeID == e.EmployeeID) == 1).ToList();

            var employeesLazy = context.Employees
               .Where(e => e.EmployeesProjects.Count(ep => ep.EmployeeID == e.EmployeeID) == 1).ToList();

            foreach (var employee in employeesEager)
            {
                string result = $"{employee.FirstName} - {employee.Department.Name}";
            }

            foreach (var employee in employeesLazy)
            {
                string result = $"{employee.FirstName} - {employee.Department.Name}";
            }
        }

        // employees1: ToList is first and OrderBy is second - the SQL query is NOT using Join statement and the performance is faster:
        // employees2: OrderBy is first and ToList is second - the SQL query is using Join statement and the performance is slower:
        private static void UsingOrderByAndToList(EmployeeContext context)
        {
            var employees1 = context.Employees.ToList().OrderBy(e => e.Department.Name).ThenBy(e => e.FirstName);

            var employees2 = context.Employees.OrderBy(e => e.Department.Name).ThenBy(e => e.FirstName).ToList();
        }

        // The Where query should be first and ToList() should be after it for better performance: 
        private static void QueryOpitmizing(EmployeeContext context)
        {
            var employees = context.Employees
                .Where(e => e.Subordinates.Any(s => s.Address.Town.Name.StartsWith("B")))
                .ToList()
                .Distinct();

            foreach (Employee e in employees)
            {
                string result = $"{e.FirstName}";
            }
        }
    }
}
