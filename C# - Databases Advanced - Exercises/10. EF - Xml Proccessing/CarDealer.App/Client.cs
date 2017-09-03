namespace CarDealer.App
{
    using Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Xml.Linq;

    public class Client
    {
        private const string SuppliersPath = "../../ImportXml/suppliers.xml";
        private const string PartsPath = "../../ImportXml/parts.xml";
        private const string CarsPath = "../../ImportXml/cars.xml";
        private const string CustomersPath = "../../ImportXml/customers.xml";

        public static void Main(string[] args)
        {
            // Task 5. Import Data - the imported data from the XML resource files is in 'Client_Seed.cs' file

            if (!Database.Exists("CarDealerContext"))
            {
                ClientSeed.ImportSuppliers(SuppliersPath);
                ClientSeed.ImportParts(PartsPath);
                ClientSeed.ImportCars(CarsPath);
                ClientSeed.ImportCustomers(CustomersPath);
                ClientSeed.ImportSales();
            }

            // Task 6.1 - Cars
            //QueryCars();

            // Task 6.2 - Cars From Make Ferrari
            //QueryCarsFromMakeFerrari();

            // Task 6.3 - Local Suppliers
            //QueryLocalSuppliers();

            // Task 6.4 - Cars With Their List Of Parts
            //QueryCarsWithTheirListOfParts();

            // Task 6.5 - Total Sales By Customer
            //QueryTotalSalesByCustomer();

            // Task 6.6 - Sales With Applied Discount
            //QuerySalesWithAppliedDiscount();
        }

        private static void QueryCars()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var cars = context.Cars
                    .Where(c => c.TravelledDistance >= 2000000)
                    .OrderBy(c => c.Make)
                    .ThenBy(c => c.Model);

                XDocument documentXml = new XDocument();

                XElement carListXml = new XElement("cars");

                foreach (var car in cars)
                {
                    XElement carXml = new XElement("car");
                    carXml.SetElementValue("make", car.Make);
                    carXml.SetElementValue("model", car.Model);
                    carXml.SetElementValue("travelled-distance", car.TravelledDistance);

                    carListXml.Add(carXml);
                }

                documentXml.Add(carListXml);
                documentXml.Save("../../ExportXml/cars.xml");
            }
        }

        private static void QueryCarsFromMakeFerrari()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
                var cars = context.Cars
                   .Where(c => c.Make == "Ferrari")
                   .OrderBy(c => c.Model)
                   .ThenByDescending(c => c.TravelledDistance)
                   .Select(c => new
                   {
                       c.Id,
                       c.Model,
                       c.TravelledDistance
                   });

                XDocument documentXml = new XDocument();

                XElement carListXml = new XElement("cars");

                foreach (var car in cars)
                {
                    XElement carXml = new XElement("car");
                    carXml.SetAttributeValue("id", car.Id);
                    carXml.SetAttributeValue("model", car.Model);
                    carXml.SetAttributeValue("travelled-distance", car.TravelledDistance);

                    carListXml.Add(carXml);
                }

                documentXml.Add(carListXml);
                documentXml.Save("../../ExportXml/carsFromMakeFerrari.xml");
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

                XDocument documentXml = new XDocument();

                XElement supplierListXml = new XElement("suppliers");

                foreach (var supplier in suppliers)
                {
                    XElement supplierXml = new XElement("supplier");
                    supplierXml.SetAttributeValue("id", supplier.Id);
                    supplierXml.SetAttributeValue("name", supplier.Name);
                    supplierXml.SetAttributeValue("parts-count", supplier.PartsCount);

                    supplierListXml.Add(supplierXml);
                }

                documentXml.Add(supplierListXml);
                documentXml.Save("../../ExportXml/localSuppliers.xml");
            }
        }

        private static void QueryCarsWithTheirListOfParts()
        {
            using (CarDealerContext context = new CarDealerContext())
            {
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

                XDocument documentXml = new XDocument();

                XElement carListXml = new XElement("cars");

                foreach (var car in cars)
                {
                    XElement carXml = new XElement("car");
                    carXml.SetAttributeValue("make", car.car.Make);
                    carXml.SetAttributeValue("model", car.car.Model);
                    carXml.SetAttributeValue("travelled-distance", car.car.TravelledDistance);

                    XElement partListXml = new XElement("parts");

                    foreach (var part in car.parts)
                    {
                        XElement partXml = new XElement("part");
                        partXml.SetAttributeValue("name", part.Name);
                        partXml.SetAttributeValue("price", part.Price);

                        partListXml.Add(partXml);
                    }

                    carXml.Add(partListXml);
                    carListXml.Add(carXml);
                }

                documentXml.Add(carListXml);
                documentXml.Save("../../ExportXml/carsWithTheirListOfParts.xml");
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

                XDocument documentXml = new XDocument();

                XElement customerListXml = new XElement("customers");

                foreach (var customer in customers)
                {
                    XElement customerXml = new XElement("customer");
                    customerXml.SetAttributeValue("full-name", customer.fullName);
                    customerXml.SetAttributeValue("bought-cars", customer.boughtCars);
                    customerXml.SetAttributeValue("spent-money", customer.spentMoney);

                    customerListXml.Add(customerXml);
                }

                documentXml.Add(customerListXml);
                documentXml.Save("../../ExportXml/totalSalesByCustomer.xml");
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

                XDocument documentXml = new XDocument();

                XElement saleListXml = new XElement("sales");

                foreach (var sale in sales)
                {
                    XElement saleXml = new XElement("sale");

                    XElement carXml = new XElement("car");
                    carXml.SetAttributeValue("make", sale.car.Make);
                    carXml.SetAttributeValue("model", sale.car.Model);
                    carXml.SetAttributeValue("travelled-distance", sale.car.TravelledDistance);

                    saleXml.Add(carXml);
                    saleXml.SetElementValue("customer-name", sale.customerName);
                    saleXml.SetElementValue("discount", sale.discount);
                    saleXml.SetElementValue("price", sale.price);
                    saleXml.SetElementValue("price-with-discount", sale.priceWithDiscount);

                    saleListXml.Add(saleXml);
                }

                documentXml.Add(saleListXml);
                documentXml.Save("../../ExportXml/salesWithAppliedDiscount.xml");
            }
        }
    }
}