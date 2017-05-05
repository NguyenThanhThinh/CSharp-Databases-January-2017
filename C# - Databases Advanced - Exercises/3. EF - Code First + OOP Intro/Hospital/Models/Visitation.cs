namespace Hospital.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System;

    public class Visitation
    {
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Comments { get; set; }

        [Required]
        public virtual Patient Patient { get; set; }
        
        public virtual Doctor Doctor { get; set; }
    }
}
