using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class RemoveTowns
    {
        public static void RemoveTown(SoftuniContext context)
        {
            string townName = Console.ReadLine();
            var town = context.Towns.First(x => x.Name == townName);

            var addressesToRemove = town.Addresses.ToList();

            foreach (var address in addressesToRemove)
            {
                var employeesAddresses = address.Employees.ToList();
                foreach (var employee in employeesAddresses)
                {
                    employee.AddressID = null;
                }
            }

            context.Addresses.RemoveRange(addressesToRemove);
            context.Towns.Remove(town);
            context.SaveChanges();

            if (addressesToRemove.Count == 1)
                Console.WriteLine($"{addressesToRemove.Count} address in {townName} was deleted");
            else
                Console.WriteLine($"{addressesToRemove.Count} addresses in {townName} were deleted");
        }
    }
}
