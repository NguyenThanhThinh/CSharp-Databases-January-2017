namespace FootballBetting.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Town
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }
    }
}
