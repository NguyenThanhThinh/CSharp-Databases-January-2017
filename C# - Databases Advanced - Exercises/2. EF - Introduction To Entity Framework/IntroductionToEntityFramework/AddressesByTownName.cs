﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class AddressesByTownName
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