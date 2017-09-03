namespace LocalStore
{
    using Models;
    using System.Collections.Generic;

    internal class StartUp
    {
        private static void Main(string[] args)
        {
            //1. Local Store - 3 products entered in the database;
            //2. Local Store Improvement - Weight and Quantity added to Product model - old data entered from 'Data_BackUp.sql';

            var context = new LocalStoreContext();

            List<Product> products = new List<Product>();

            products.Add(new Product()
            {
                Name = "Cucumber",
                DistributorName = "Local Street Market",
                Description = "Product From Bulgaria",
                Price = 1.99m
            });

            products.Add(new Product()
            {
                Name = "Tomato",
                DistributorName = "Local Street Market",
                Description = "Product From Bulgaria",
                Price = 1.49m
            });

            products.Add(new Product()
            {
                Name = "Potato",
                DistributorName = "Local Street Market",
                Description = "Product From Bulgaria",
                Price = 0.99m
            });

            foreach (Product product in products)
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }
    }
}