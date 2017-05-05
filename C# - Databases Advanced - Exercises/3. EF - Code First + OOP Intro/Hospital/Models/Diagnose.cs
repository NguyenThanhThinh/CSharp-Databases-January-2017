namespace Hospital.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Diagnose
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Comments { get; set; }

        [Required]
        public virtual Patient Patient { get; set; }
    }
}
