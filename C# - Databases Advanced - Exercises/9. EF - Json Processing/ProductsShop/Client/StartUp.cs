namespace ProductsShop.Client
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

            // Task 1. - Create Database
            // Task 2. - Import/Seed Data

            if (!Database.Exists("ProductsShopContext"))
            {
                SeedUsers();
                SeedProducts();
                SeedCategories();
            }

            // Task 3.1 - Products In Range
            //QueryProductsInRange();

            // Task 3.2 - Successfully Sold Products
            //QuerySuccessfullySoldProducts();

            // Task 3.3 - Categories By Products Count
            //QueryCategoriesByProductsCount();

            // Task 3.4 - Users and Products
            //QueryUsersAndProducts();
        }

        // A method which converts given <TEntity> collection to JSON and then saves it in "ExportJson" folder
        private static void ExportJsonToFolder<TEntity>(TEntity entityType, string pathToExport)
        {
            string json = JsonConvert.SerializeObject(entityType, Formatting.Indented);
            File.WriteAllText(pathToExport, json);
            Console.WriteLine(json);
        }

        private static void QueryProductsInRange()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var productsInRange = context.Products
                    .Where(p => p.Price >= 500 && p.Price <= 1000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        seller = p.Seller.FirstName + " " + p.Seller.LastName
                    });

                ExportJsonToFolder(productsInRange, "../../ExportJson/productsInRange.json");
            }
        }

        private static void QuerySuccessfullySoldProducts()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.ProductsSold.Count(p => p.Buyer != null) != 0)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        soldProducts = u.ProductsSold.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                    });

                ExportJsonToFolder(users, "../../ExportJson/successfullySoldProducts.json");
            }
        }

        private static void QueryCategoriesByProductsCount()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var categories = context.Categories
                    .OrderBy(cat => cat.Name)
                    .Select(c => new
                    {
                        name = c.Name,
                        productsCount = c.Products.Count,
                        averagePrice = c.Products.Average(p => p.Price),
                        totalRevenue = c.Products.Sum(p => p.Price)
                    });

                ExportJsonToFolder(categories, "../../ExportJson/categoriesByProductCout.json");
            }
        }

        private static void QueryUsersAndProducts()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.ProductsSold.Count > 0)
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .ThenBy(u => u.LastName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        age = u.Age,
                        soldProducts = new
                        {
                            count = u.ProductsSold.Count,
                            products = u.ProductsSold.Select(p => new
                            {
                                name = p.Name,
                                price = p.Price
                            })
                        }
                    });

                var usersToSerialize = new
                {
                    usersCount = users.Count(),
                    users
                };

                ExportJsonToFolder(usersToSerialize, "../../ExportJson/usersAndProducts.json");
            }
        }

        private static void SeedUsers()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                string jsonUsers = File.ReadAllText("../../ImportJson/users.json");
                List<User> users = JsonConvert.DeserializeObject<List<User>>(jsonUsers);

                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        private static void SeedProducts()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                string jsonProducts = File.ReadAllText("../../ImportJson/products.json");
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonProducts);

                int countOfUsers = context.Users.Count();

                Random rnd = new Random();
                foreach (Product product in products)
                {
                    product.SellerId = rnd.Next(1, countOfUsers + 1);
                    if (product.SellerId % 5 != 0 && product.SellerId % 10 != 0)
                    {
                        product.BuyerId = rnd.Next(1, countOfUsers + 1);
                    }
                }

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }

        private static void SeedCategories()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                string jsonCategories = File.ReadAllText("../../ImportJson/categories.json");
                List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(jsonCategories);

                int countOfProducts = context.Products.Count();

                Random rnd = new Random();
                foreach (Category category in categories)
                {
                    for (int i = 0; i < countOfProducts / 3; i++)
                    {
                        Product product = context.Products.Find(rnd.Next(1, countOfProducts + 1));
                        category.Products.Add(product);
                    }
                }

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}