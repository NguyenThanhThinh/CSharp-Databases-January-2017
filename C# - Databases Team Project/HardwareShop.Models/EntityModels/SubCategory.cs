namespace HardwareShop.Models.EntityModels
{
    using System.Collections.Generic;

    public class SubCategory
    {
        public SubCategory()
        {
            this.Items = new HashSet<Item>();
        }

        public int SubCategoryId { get; set; }

        public string Name { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}