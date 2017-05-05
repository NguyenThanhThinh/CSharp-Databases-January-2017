namespace HardwareShop.Models.EntityModels
{
    using System.Collections.Generic;

    public class Category
    {
        public Category()
        {
            this.SubCategories = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<SubCategory> SubCategories { get; set; }
    }
}