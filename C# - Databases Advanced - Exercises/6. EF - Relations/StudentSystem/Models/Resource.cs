namespace StudentSystem.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public enum TypeOfResource
    {
        Video,
        Presentation,
        Document,
        Other
    }

    public class Resource
    {
        public Resource()
        {
            this.Licenses = new HashSet<License>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public TypeOfResource Type { get; set; }

        [Required]
        public string URL { get; set; }

        public virtual Course Course { get; set; }

        public virtual ICollection<License> Licenses { get; set; }
    }
}
