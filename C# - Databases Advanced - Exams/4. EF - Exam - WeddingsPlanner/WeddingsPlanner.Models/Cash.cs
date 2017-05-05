namespace WeddingsPlanner.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Cash")]
    public class Cash : Present
    {
        [Required]
        public decimal Amount { get; set; }
    }
}
