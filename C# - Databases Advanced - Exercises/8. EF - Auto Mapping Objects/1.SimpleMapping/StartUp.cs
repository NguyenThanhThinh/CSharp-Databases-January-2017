namespace _1.SimpleMapping
{
    using AutoMapper;
    using DTOs;
    using Models;
    using System;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            ConfigureAutoMapper();

            Employee employee = new Employee()
            {
                FirstName = "Tom",
                LastName = "Hanks",
                Address = "L.A. Caifornia",
                BirthDate = new DateTime(1956, 07, 09),
                Salary = 1000000m
            };

            EmployeeDTO employeeDto = Mapper.Map<Employee, EmployeeDTO>(employee);

            Console.WriteLine($"{employeeDto.LastName} {employeeDto.LastName} - {employeeDto.Salary}");
        }

        private static void ConfigureAutoMapper()
        {
            Mapper.Initialize(config => config.CreateMap<Employee, EmployeeDTO>());
        }
    }
}