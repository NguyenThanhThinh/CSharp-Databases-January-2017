namespace Sales.Models
{
    using System.Collections.Generic;

    public class Customer
    {
        public Customer()
        {
            this.SalesForCustomer = new HashSet<Sale>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; } = 20;

        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public ICollection<Sale> SalesForCustomer { get; set; }
    }
}
