namespace _3.Projection
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using DTOs;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            ConfigureAutoMapper();

            List<Employee> employees = CreateEmployeesAndManagers();

            EmployeeContext context = new EmployeeContext();
            context.Employees.AddRange(employees);
            context.SaveChanges();

            var employeesBefore1990Dto = context.Employees
                .Where(e => e.Birthday.Year < 1990)
                .OrderByDescending(e => e.Salary)
                .ProjectTo<EmployeeDTO>();

            foreach (EmployeeDTO employee in employeesBefore1990Dto)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.Salary} - Manager: {employee.ManagerLastName ?? "[no manager]"}");
            }
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.Initialize(config => config.CreateMap<Employee, EmployeeDTO>()
                        .ForMember(dto => dto.ManagerLastName, opt => opt.MapFrom(src => src.Manager.LastName)));
        }

        private static List<Employee> CreateEmployeesAndManagers()
        {
            List<Employee> employees = new List<Employee>();

            Employee manager = new Employee()
            {
                FirstName = "Steve",
                LastName = "Jobbsen",
                Salary = 1000000,
                Birthday = new DateTime(1980, 08, 08)
            };
            Employee manager2 = new Employee()
            {
                FirstName = "Carl",
                LastName = "Kormac",
                Salary = 1000000,
                Birthday = new DateTime(1980, 08, 08)
            };
            Employee employee1 = new Employee()
            {
                FirstName = "Stephen",
                LastName = "Bjorn",
                Salary = 4300.00m,
                Manager = manager,
                Birthday = new DateTime(1980, 08, 08)
            };
            Employee employee2 = new Employee()
            {
                FirstName = "Kirilyc",
                LastName = "Lefi",
                Salary = 4400.00m,
                Manager = manager,
                Birthday = new DateTime(1990, 08, 08)
            };
            Employee employee3 = new Employee()
            {
                FirstName = "Jurgen",
                LastName = "Straus",
                Salary = 1000.45m,
                Manager = manager2,
                Birthday = new DateTime(1980, 08, 08)
            };
            Employee employee4 = new Employee()
            {
                FirstName = "Moni",
                LastName = "Kozinac",
                Salary = 2030.99m,
                Manager = manager2,
                Birthday = new DateTime(1990, 08, 08)
            };
            Employee employee5 = new Employee()
            {
                FirstName = "Kopp",
                LastName = "Spidok",
                Salary = 2000.21m,
                Manager = manager2,
                Birthday = new DateTime(1990, 08, 08)
            };

            employees.Add(manager);
            employees.Add(manager2);
            employees.Add(employee1);
            employees.Add(employee2);
            employees.Add(employee3);
            employees.Add(employee4);
            employees.Add(employee5);

            return employees;
        }
    }
}