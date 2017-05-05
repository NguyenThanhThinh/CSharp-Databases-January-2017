namespace ProductsShop
{
    using Data;
    using Model;
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class ClientSeed
    {
        public static void ImportUsers(string usersPath)
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                XDocument xmlDocument = XDocument.Load(usersPath);

                //var users = xmlDocument.Root.Elements().Select(ParseUser).ToList();
                var users = xmlDocument.XPathSelectElements("users/user").Select(ParseUser).ToList();
                
                context.Users.AddRange(users);
                context.SaveChanges();
            }
        }

        public static void ImportProducts(string productsPath)
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                XDocument xmlDocument = XDocument.Load(productsPath);

                //var products = xmlDocument.Root.Elements().Select(ParseProduct).ToList();
                var products = xmlDocument.XPathSelectElements("products/product").Select(ParseProduct).ToList();

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

        public static void ImportCategories(string categoriesPath)
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                XDocument xmlDocument = XDocument.Load(categoriesPath);

                //var categories = xmlDocument.Root.Elements().Select(ParseCategory).ToList();
                var categories = xmlDocument.XPathSelectElements("categories/category").Select(ParseCategory).ToList();

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

        public static User ParseUser(XElement userToParse)
        {
            string firstName = userToParse.Attribute("first-name")?.Value;
            string lastName = userToParse.Attribute("last-name")?.Value;
            int age = 0;

            if (userToParse.Attribute("age") != null)
            {
                age = int.Parse(userToParse.Attribute("age").Value);
            }

            User parsedUser;

            if (firstName == null)
            {
                parsedUser = new User()
                {
                    LastName = lastName,
                    Age = age
                };
            }
            else
            {
                parsedUser = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };
            }

            return parsedUser;
        }


        public static Product ParseProduct(XElement productToParse)
        {
            string name = productToParse.Element("name")?.Value;
            decimal price = decimal.Parse(productToParse.Element("price").Value);

            Product parsedProduct = new Product()
            {
                Name = name,
                Price = price
            };

            return parsedProduct;
        }

        public static Category ParseCategory(XElement categoryToParse)
        {
            string name = categoryToParse.Element("name")?.Value;

            Category parsedCategory = new Category()
            {
                Name = name
            };

            return parsedCategory;
        }
    }
}
