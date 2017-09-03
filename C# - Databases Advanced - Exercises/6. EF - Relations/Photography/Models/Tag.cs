namespace Photography.Models
{
    using PhotographersDB;
    using System.Collections.Generic;

    public class Tag
    {
        public Tag()
        {
            this.Albums = new HashSet<Album>();
        }

        public int Id { get; set; }

        [Tag]
        public string Name { get; set; }

        public virtual ICollection<Album> Albums { get; set; }
    }
}