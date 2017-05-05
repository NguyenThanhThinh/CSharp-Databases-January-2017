using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class AddNewAddressAndUpdateEmployee
    {
        public static void AddingNewAddressAndUpdateEmployee(SoftuniContext context)
        {
            string newAddressText = "Vitoshka 15";
            int newAddressTownId = 4;

            var address = new Address()
            {
                AddressText = newAddressText,
                TownID = newAddressTownId
            };

            Employee employee = context.Employees.FirstOrDefault(x => x.LastName == "Nakov");
            if (employee != null)
            {
                employee.Address = address;
            }
            context.SaveChanges();

            var employees = context.Employees.OrderByDescending(x => x.AddressID)
                                             .Take(10);
            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Address.AddressText}");
            }
        }
    }
}
