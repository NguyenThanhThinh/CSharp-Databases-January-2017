namespace Sales.Models
{
    using System.Collections.Generic;

    public class Product
    {
        public Product()
        {
            this.SalesOfProduct = new HashSet<Sale>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public double Quantity { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = "No description";

        public ICollection<Sale> SalesOfProduct { get; set; }
    }
}