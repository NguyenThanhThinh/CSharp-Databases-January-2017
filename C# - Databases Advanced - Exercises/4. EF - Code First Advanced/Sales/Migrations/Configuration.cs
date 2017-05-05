namespace Sales.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Models;

    internal sealed class Configuration : DbMigrationsConfiguration<SalesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Sales.SalesContext";
        }

        protected override void Seed(SalesContext context)
        {
            Product product = new Product()
            {
                Name = "Opel",
                Price = 10000,
                Quantity = 10
            };
            context.Products.AddOrUpdate(product);

            Product product2 = new Product()
            {
                Name = "VW",
                Price = 20000,
                Quantity = 20
            };
            context.Products.AddOrUpdate(product2);

            Product product3 = new Product()
            {
                Name = "Ford",
                Price = 30000,
                Quantity = 30
            };
            context.Products.AddOrUpdate(product3);

            Customer customer = new Customer()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "abv@abv.bg",
                CreditCardNumber = "1234 5678 1234 5678"
            };
            context.Customers.AddOrUpdate(customer);

            Customer customer2 = new Customer()
            {
                FirstName = "Dragan",
                LastName = "Draganov",
                Email = "abv@abv.bg",
                CreditCardNumber = "1234 5678 1234 5678"
            };
            context.Customers.AddOrUpdate(customer2);

            Customer customer3 = new Customer()
            {
                FirstName = "Stamen",
                LastName = "Stamenov",
                Email = "abv@abv.bg",
                CreditCardNumber = "1234 5678 1234 5678"
            };
            context.Customers.AddOrUpdate(customer3);
            
            StoreLocation location = new StoreLocation()
            {
                LocationName = "Sofia"
            };
            context.StoreLocations.AddOrUpdate(location);

            StoreLocation location2 = new StoreLocation()
            {
                LocationName = "Montana"
            };
            context.StoreLocations.AddOrUpdate(location2);

            StoreLocation location3 = new StoreLocation()
            {
                LocationName = "Veliko Tarnovo"
            };
            context.StoreLocations.AddOrUpdate(location3);

            Sale sale = new Sale()
            {
                Product = product,
                Customer = customer,
                Location = location,
                Date = DateTime.Now
            };
            context.Sales.AddOrUpdate(sale);

            Sale sale2 = new Sale()
            {
                Product = product2,
                Customer = customer2,
                Location = location2,
                Date = DateTime.Now
            };
            context.Sales.AddOrUpdate(sale2);

            Sale sale3 = new Sale()
            {
                Product = product3,
                Customer = customer3,
                Location = location3,
                Date = DateTime.Now
            };
            context.Sales.AddOrUpdate(sale3);
        }
    }
}
