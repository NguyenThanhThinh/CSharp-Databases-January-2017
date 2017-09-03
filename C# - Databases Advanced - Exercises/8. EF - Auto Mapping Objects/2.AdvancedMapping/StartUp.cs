namespace _2.AdvancedMapping
{
    using AutoMapper;
    using DTOs;
    using Models;
    using System;
    using System.Collections.Generic;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            ConfigureAutoMapper();

            List<Employee> managers = CreateEmployeesAndManagers();

            ManagerDTO[] managersDto = Mapper.Map<List<Employee>, ManagerDTO[]>(managers);

            foreach (ManagerDTO managerDto in managersDto)
            {
                Console.WriteLine($"{managerDto.FirstName} {managerDto.LastName} | Employees: {managerDto.SubordinatesCount}");
                foreach (EmployeeDTO employeeDto in managerDto.Subordinates)
                {
                    Console.WriteLine($"    - {employeeDto.FirstName} {employeeDto.LastName} {employeeDto.Salary}");
                }
            }
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Employee, EmployeeDTO>();
                config.CreateMap<Employee, ManagerDTO>()
                      .ForMember(dto => dto.SubordinatesCount, opt => opt.MapFrom(src => src.Subordinates.Count));
            });
        }

        private static List<Employee> CreateEmployeesAndManagers()
        {
            List<Employee> managers = new List<Employee>();

            Employee manager = new Employee()
            {
                FirstName = "Steve",
                LastName = "Jobbsen",
                Salary = 1000000,
                Subordinates = new List<Employee>()
            };
            Employee manager2 = new Employee()
            {
                FirstName = "Carl",
                LastName = "Kormac",
                Salary = 1000000,
                Subordinates = new List<Employee>()
            };
            Employee employee1 = new Employee()
            {
                FirstName = "Stephen",
                LastName = "Bjorn",
                Salary = 4300.00m,
                Manager = manager
            };
            Employee employee2 = new Employee()
            {
                FirstName = "Kirilyc",
                LastName = "Lefi",
                Salary = 4400.00m,
                Manager = manager
            };
            Employee employee3 = new Employee()
            {
                FirstName = "Jurgen",
                LastName = "Straus",
                Salary = 1000.45m,
                Manager = manager2
            };
            Employee employee4 = new Employee()
            {
                FirstName = "Moni",
                LastName = "Kozinac",
                Salary = 2030.99m,
                Manager = manager2
            };
            Employee employee5 = new Employee()
            {
                FirstName = "Kopp",
                LastName = "Spidok",
                Salary = 2000.21m,
                Manager = manager2
            };

            manager.Subordinates.Add(employee1);
            manager.Subordinates.Add(employee2);
            manager2.Subordinates.Add(employee3);
            manager2.Subordinates.Add(employee4);
            manager2.Subordinates.Add(employee5);

            managers.Add(manager);
            managers.Add(manager2);

            return managers;
        }
    }
}