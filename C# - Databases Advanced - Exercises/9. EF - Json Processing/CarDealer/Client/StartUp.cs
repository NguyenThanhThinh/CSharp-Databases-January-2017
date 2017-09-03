namespace CarDealer.Client
{
    using Data;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            // You may have different App.config settings
            // JSON import files are in the project solution - ImportJson folder
            // JSON result files from each task are saved in the project solution - ExportJson folder

            // Task 4. - Create Database
            // Task 5. - Import/Seed Data

            if (!Database.Exists("CarDealerContext"))
            {
                SeedSuppliers();
                SeedParts();
                SeedCars();
                SeedCustomers();
                SeedSales();
            }

            // Task 6.1 - Ordered Customers
            //QueryOrderedCustomers();

            // Task 6.2 - Cars From Make Toyota
            //QueryCarsFromMakeToyota();

            // Task 6.3 - Local Suppliers
            //QueryLocalSuppliers();

            // Task 6.4 - Cars With Their List Of Parts
            //QueryCarsWithTheirListOfParts();

            // Task 6.5 - Total Sales By Customer
            //QueryTotalSalesByCustomer();

            // Task 6.6 - Sales with Applied Discount
            //QuerySalesWithAppliedDiscount();
        }

        // A method which converts given <TEntity> collection to JSON and then saves it in "ExportJson" folder
        private static void ExportJsonToFolder<TEntity>(TEntity entityType, string pathToExport)
        {
            string json = JsonConvert.SerializeObject(entityType, Formatting.Indented);
            File.WriteAllText(pathToExport, json);
            Console.WriteLine(json);
        }

        private static void QueryOrderedCustomers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var customers = context.Customers
                    .OrderBy(c => c.BirthDate)
                    .ThenByDescending(c => c.IsYoungDriver);

                ExportJsonToFolder(customers, "../../ExportJson/orderedCustomers.json");
            }
        }

        private static void QueryCarsFromMakeToyota()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var cars = context.Cars
                    .Where(c => c.Make == "Toyota")
                    .OrderBy(c => c.Model)
                    .ThenByDescending(c => c.TravelledDistance)
                    .Select(c => new
                    {
                        c.Id,
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    });

                ExportJsonToFolder(cars, "../../ExportJson/carsFromMakeToyota.json");
            }
        }

        private static void QueryLocalSuppliers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var suppliers = context.Suppliers
                    .Where(s => s.IsImporter == false)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        PartsCount = s.Parts.Count
                    });

                ExportJsonToFolder(suppliers, "../../ExportJson/localSuppliers.json");
            }
        }

        private static void QueryCarsWithTheirListOfParts()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                Console.SetBufferSize(Console.BufferWidth, 32766);

                var cars = context.Cars
                    .Select(c => new
                    {
                        car = new
                        {
                            c.Make,
                            c.Model,
                            c.TravelledDistance
                        },
                        parts = c.Parts.Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                    });

                ExportJsonToFolder(cars, "../../ExportJson/carsWithTheirListOfParts.json");
            }
        }

        private static void QueryTotalSalesByCustomer()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var customers = context.Customers
                    .Where(c => c.Sales.Any())
                    .Select(c => new
                    {
                        fullName = c.Name,
                        boughtCars = c.Sales.Count,
                        spentMoney = c.Sales.Sum(s => s.Car.Parts.Sum(p => p.Price))
                    })
                    .OrderByDescending(c => c.spentMoney)
                    .ThenByDescending(c => c.boughtCars);

                ExportJsonToFolder(customers, "../../ExportJson/totalSalesByCustomer.json");
            }
        }

        private static void QuerySalesWithAppliedDiscount()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var sales = context.Sales
                    .Select(s => new
                    {
                        car = new
                        {
                            s.Car.Make,
                            s.Car.Model,
                            s.Car.TravelledDistance
                        },
                        customerName = s.Customer.Name,
                        discount = s.Discount,
                        price = s.Car.Parts.Sum(p => p.Price),
                        priceWithDiscount = s.Car.Parts.Sum(p => p.Price) - (s.Car.Parts.Sum(p => p.Price) * s.Discount)
                    });

                ExportJsonToFolder(sales, "../../ExportJson/salesWithAppliedDiscount.json");
            }
        }

        private static void SeedSuppliers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string jsonSuppliers = File.ReadAllText("../../ImportJson/suppliers.json");
                List<Supplier> suppliers = JsonConvert.DeserializeObject<List<Supplier>>(jsonSuppliers);

                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }
        }

        private static void SeedParts()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string jsonParts = File.ReadAllText("../../ImportJson/parts.json");
                List<Part> parts = JsonConvert.DeserializeObject<List<Part>>(jsonParts);

                Random rnd = new Random();
                List<Supplier> suppliers = context.Suppliers.ToList();
                foreach (Part part in parts)
                {
                    //part.SupplierId = rnd.Next(1, context.Suppliers.Count() + 1);
                    part.Supplier = suppliers[rnd.Next(suppliers.Count)];
                }

                context.Parts.AddRange(parts);
                context.SaveChanges();
            }
        }

        private static void SeedCars()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string jsonCars = File.ReadAllText("../../ImportJson/cars.json");
                List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(jsonCars);

                Random rnd = new Random();
                //int countOfParts = context.Parts.Count();
                List<Part> parts = context.Parts.ToList();
                foreach (Car car in cars)
                {
                    int numberOfParts = rnd.Next(10, 21);

                    for (int i = 0; i < numberOfParts; i++)
                    {
                        //Part part = context.Parts.Find(rnd.Next(1, countOfParts + 1));
                        Part part = parts[rnd.Next(parts.Count)];
                        car.Parts.Add(part);
                    }
                }

                context.Cars.AddRange(cars);
                context.SaveChanges();
            }
        }

        private static void SeedCustomers()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                string jsonCustomers = File.ReadAllText("../../ImportJson/customers.json");
                List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(jsonCustomers);

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }
        }

        private static void SeedSales()
        {
            using (var context = new CarDealerContext())
            {
                double[] discountValues = { 0, 0.05, 0.10, 0.20, 0.30, 0.40, 0.50 };

                Random rnd = new Random();
                List<Car> cars = context.Cars.ToList();
                List<Customer> customers = context.Customers.ToList();

                for (int i = 0; i < 100; i++)
                {
                    Car car = cars[rnd.Next(cars.Count)];
                    Customer customer = customers[rnd.Next(customers.Count)];
                    double discount = discountValues[rnd.Next(discountValues.Length)];

                    if (customer.IsYoungDriver)
                    {
                        discount += 0.05;
                    }

                    Sale sale = new Sale()
                    {
                        Car = car,
                        Customer = customer,
                        Discount = discount
                    };

                    context.Sales.Add(sale);
                }

                context.SaveChanges();
            }
        }
    }
}