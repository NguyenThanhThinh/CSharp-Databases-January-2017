namespace WeddingsPlanner.Models
{
    using Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Gifts")]
    public class Gift : Present
    {
        [Required]
        public string Name { get; set; }

        public PresentSize? PresentSize { get; set; }
    }
}
