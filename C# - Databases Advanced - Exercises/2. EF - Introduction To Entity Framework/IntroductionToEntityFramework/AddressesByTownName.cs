using System;
using System.Linq;

namespace IntroductionToEntityFramework
{
    internal class AddressesByTownName
    {
        public static void GetAddressesByTownName(SoftuniContext context)
        {
            var addresses = context.Addresses.OrderByDescending(x => x.Employees.Count)
                                             .ThenBy(x => x.Town.Name)
                                             .Take(10);

            foreach (var address in addresses)
            {
                Console.WriteLine($"{address.AddressText}, {address.Town.Name} - {address.Employees.Count} employees");
            }
        }
    }
}