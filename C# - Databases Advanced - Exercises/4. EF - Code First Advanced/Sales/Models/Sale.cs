namespace Sales.Models
{
    using System;

    public class Sale
    {
        public int Id { get; set; }

        public Product Product { get; set; }

        public Customer Customer { get; set; }

        public StoreLocation Location { get; set; }

        public DateTime Date { get; set; }
    }
}