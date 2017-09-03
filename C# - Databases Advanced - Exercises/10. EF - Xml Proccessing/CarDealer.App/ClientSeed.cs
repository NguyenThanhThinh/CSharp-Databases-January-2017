namespace CarDealer.App
{
    using Data;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    internal class ClientSeed
    {
        public static void ImportSuppliers(string suppliersPath)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                XDocument xmlDocument = XDocument.Load("../../ImportXml/suppliers.xml");

                //List<Supplier> suppliers = xmlDocument.Root.Elements().Select(ParseSupplier).ToList();
                List<Supplier> suppliers = xmlDocument.XPathSelectElements("suppliers/supplier").Select(ParseSupplier).ToList();

                context.Suppliers.AddRange(suppliers);
                context.SaveChanges();
            }
        }

        public static void ImportParts(string partsPath)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                XDocument xmlDocument = XDocument.Load("../../ImportXml/parts.xml");

                //List<Part> parts = xmlDocument.Root.Elements().Select(ParsePart).ToList();
                List<Part> parts = xmlDocument.XPathSelectElements("parts/part").Select(ParsePart).ToList();

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

        public static void ImportCars(string carsPath)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                XDocument xmlDocument = XDocument.Load(carsPath);

                //List<Car> cars = xmlDocument.Root.Elements().Select(ParseCar).ToList();
                List<Car> cars = xmlDocument.XPathSelectElements("cars/car").Select(ParseCar).ToList();

                Random rnd = new Random();
                int countOfParts = context.Parts.Count();
                //List<Part> parts = context.Parts.ToList();
                foreach (Car car in cars)
                {
                    int numberOfParts = rnd.Next(10, 21);

                    for (int i = 0; i < numberOfParts; i++)
                    {
                        //Part part = parts[rnd.Next(parts.Count)];
                        Part part = context.Parts.Find(rnd.Next(1, countOfParts + 1));
                        car.Parts.Add(part);
                    }
                }

                context.Cars.AddRange(cars);
                context.SaveChanges();
            }
        }

        public static void ImportCustomers(string customersPath)
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                XDocument xmlDocument = XDocument.Load(customersPath);

                //List<Customer> customers = xmlDocument.Root.Elements().Select(ParseCustomer).ToList();
                List<Customer> customers = xmlDocument.XPathSelectElements("customers/customer").Select(ParseCustomer).ToList();

                context.Customers.AddRange(customers);
                context.SaveChanges();
            }
        }

        public static void ImportSales()
        {
            using (var context = new CarDealerContext())
            {
                decimal[] discountValues = { 0.0m, 0.05m, 0.10m, 0.20m, 0.30m, 0.40m, 0.50m };

                Random rnd = new Random();
                List<Car> cars = context.Cars.ToList();
                List<Customer> customers = context.Customers.ToList();

                for (int i = 0; i < 100; i++)
                {
                    Car car = cars[rnd.Next(cars.Count)];
                    Customer customer = customers[rnd.Next(customers.Count)];
                    decimal discount = discountValues[rnd.Next(discountValues.Length)];

                    if (customer.IsYoungDriver)
                    {
                        discount += 0.05m;
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

        private static Supplier ParseSupplier(XElement supplierToParse)
        {
            string name = supplierToParse.Attribute("name")?.Value;
            bool isImporter = bool.Parse(supplierToParse.Attribute("is-importer").Value);

            Supplier supplier = new Supplier()
            {
                Name = name,
                IsImporter = isImporter
            };

            return supplier;
        }

        private static Part ParsePart(XElement partToParse)
        {
            string name = partToParse.Attribute("name")?.Value;
            decimal price = decimal.Parse(partToParse.Attribute("price").Value);
            int quantity = int.Parse(partToParse.Attribute("quantity").Value);

            Part part = new Part()
            {
                Name = name,
                Price = price,
                Quantity = quantity
            };

            return part;
        }

        private static Car ParseCar(XElement carToParse)
        {
            string make = carToParse.Element("make")?.Value;
            string model = carToParse.Element("model")?.Value;
            long travelledDistance = long.Parse(carToParse.Element("travelled-distance").Value);

            Car car = new Car()
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistance
            };

            return car;
        }

        private static Customer ParseCustomer(XElement customerToParse)
        {
            string name = customerToParse.Attribute("name")?.Value;
            DateTime birthDate = DateTime.Parse(customerToParse.Element("birth-date")?.Value);
            bool isYoungDriver = bool.Parse(customerToParse.Element("is-young-driver").Value);

            Customer customer = new Customer()
            {
                Name = name,
                BirthDate = birthDate,
                IsYoungDriver = isYoungDriver
            };

            return customer;
        }
    }
}