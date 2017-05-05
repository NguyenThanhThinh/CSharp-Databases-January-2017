namespace Performance.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Town
    {
        public Town()
        {
            Addresses = new HashSet<Address>();
        }

        public int TownID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
