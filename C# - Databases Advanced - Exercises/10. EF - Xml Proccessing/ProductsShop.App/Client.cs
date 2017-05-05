namespace ProductsShop
{
    using Data;
    using System.Linq;
    using System.Xml.Linq;
    using System.Data.Entity;

    public class Client
    {
        private const string CategoriesPath = "../../ImportXml/categories.xml";
        private const string ProductsPath = "../../ImportXml/products.xml";
        private const string UsersPath = "../../ImportXml/users.xml";

        public static void Main(string[] args)
        {
            // Task 1. Create Students XML Document - the solution is in folder Task1_and_Task2 - 'students.xml'
            // Task 2. Catalog of Musical Albums in XML Format - the solution is in folder Task1_and_Task2 - 'musicalAlbumsCatalog.xml'
            // Task 3. Import Data - the imported data from the XML resource files is in 'Client_Seed.cs' file 

            if (!Database.Exists("ProductsShopContext"))
            {
                ClientSeed.ImportUsers(UsersPath);
                ClientSeed.ImportProducts(ProductsPath);
                ClientSeed.ImportCategories(CategoriesPath);
            }

            // Task 4.1 Products In Range
            //QueryProductInRange();

            // Task 4.2 Sold Products
            //QuerySuccessfullySoldProducts();

            // Task 4.3 Categories By Products Count
            //QueryCategoriesByProductsCount();

            // Task 4.4 UsersAndProducts
            //QueryUsersAndProducts();
        }

        private static void QueryProductInRange()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var productsInRange = context.Products
                    .Where(p => p.BuyerId != null && p.Price >= 1000 && p.Price <= 2000)
                    .OrderBy(p => p.Price)
                    .Select(p => new
                    {
                        name = p.Name,
                        price = p.Price,
                        buyer = p.Buyer.FirstName + " " + p.Buyer.LastName
                    });

                XDocument documentXml = new XDocument();

                XElement productListXml = new XElement("products");

                foreach (var product in productsInRange)
                {
                    XElement productXml = new XElement("product");

                    productXml.SetAttributeValue("name", product.name);
                    productXml.SetAttributeValue("price", product.price);
                    productXml.SetAttributeValue("buyer", product.buyer);

                    productListXml.Add(productXml);
                }

                documentXml.Add(productListXml);
                documentXml.Save("../../ExportXml/productsInRange.xml");
            }
        }

        private static void QuerySuccessfullySoldProducts()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var users = context.Users
                    .Where(u => u.ProductsSold.Count != 0)
                    .OrderBy(u => u.LastName)
                    .ThenBy(u => u.FirstName)
                    .Select(u => new
                    {
                        firstName = u.FirstName,
                        lastName = u.LastName,
                        soldProducts = u.ProductsSold.Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                    });

                XDocument documentXml = new XDocument();

                XElement userListXml = new XElement("users");

                foreach (var user in users)
                {
                    XElement userXml = new XElement("user");

                    if (user.firstName != null)
                    {
                        userXml.SetAttributeValue("first-name", user.firstName);
                    }

                    userXml.SetAttributeValue("last-name", user.lastName);

                    XElement productListXml = new XElement("sold-products");

                    foreach (var product in user.soldProducts)
                    {
                        XElement productXml = new XElement("product");

                        productXml.SetElementValue("name", product.name);
                        productXml.SetElementValue("price", product.price);

                        productListXml.Add(productXml);
                    }

                    userXml.Add(productListXml);
                    userListXml.Add(userXml);

                }

                documentXml.Add(userListXml);
                documentXml.Save("../../ExportXml/soldProducts.xml");
            }
        }

        private static void QueryCategoriesByProductsCount()
        {
            using (ProductsShopContext context = new ProductsShopContext())
            {
                var categories = context.Categories
                    .OrderByDescending(cat => cat.Products.Count)
                    .Select(c => new
                    {
                        name = c.Name,
                        productsCount = c.Products.Count,
                        averagePrice = c.Products.Average(p => p.Price),
                        totalRevenue = c.Products.Sum(p => p.Price)
                    });

                XDocument documentXml = new XDocument();

                XElement categoryListXml = new XElement("categories");

                foreach (var category in categories)
                {
                    XElement categoryXml = new XElement("category");

                    categoryXml.SetAttributeValue("name", category.name);

                    categoryXml.SetElementValue("products-count", category.productsCount);
                    categoryXml.SetElementValue("average-price", category.averagePrice);
                    categoryXml.SetElementValue("total-revenue", category.totalRevenue);

                    categoryListXml.Add(categoryXml);
                }

                documentXml.Add(categoryListXml);
                documentXml.Save("../../ExportXml/categoriesByProductCount.xml");
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

                XDocument documentXml = new XDocument();

                XElement userListXml = new XElement("users");
                userListXml.SetAttributeValue("count", users.Count());

                foreach (var user in users)
                {
                    XElement userXml = new XElement("user");

                    if (user.firstName != null)
                    {
                        userXml.SetAttributeValue("first-name", user.firstName);
                    }

                    userXml.SetAttributeValue("last-name", user.lastName);
                    userXml.SetAttributeValue("age", user.age);
                    
                    XElement soldProductsXml = new XElement("sold-products");
                    soldProductsXml.SetAttributeValue("count", user.soldProducts.count);

                    foreach (var product in user.soldProducts.products)
                    {
                        XElement productXml = new XElement("product");
                        productXml.SetAttributeValue("name", product.name);
                        productXml.SetAttributeValue("price", product.price);

                        soldProductsXml.Add(productXml);
                    }

                    userXml.Add(soldProductsXml);
                    userListXml.Add(userXml);
                }

                documentXml.Add(userListXml);
                documentXml.Save("../../ExportXml/usersAndProducts.xml");
            }
        }
    }
}
