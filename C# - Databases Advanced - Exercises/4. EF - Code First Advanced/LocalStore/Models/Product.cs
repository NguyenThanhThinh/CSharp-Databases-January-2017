namespace LocalStore.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string DistributorName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal? Weight { get; set; }

        public int? Quantity { get; set; }
    }
}