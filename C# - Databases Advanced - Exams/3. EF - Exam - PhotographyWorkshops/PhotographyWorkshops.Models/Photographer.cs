namespace PhotographyWorkshops.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Photographer
    {
        public Photographer()
        {
            this.Lenses = new HashSet<Lens>();
            this.Accessories = new HashSet<Accessory>();    
            this.WorkshopsTrained = new HashSet<Workshop>();
            this.WorkshopsParticipated = new HashSet<Workshop>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{this.FirstName} {this.LastName}"; }
        }

        //[Phone]
        // The 'PhoneAttribute' is not working for some reason

        [RegularExpression(@"\+\d{1,3}\/\d{8,10}")]
        public string Phone { get; set; }

        public int PrimaryCameraId { get; set; }

        [ForeignKey("PrimaryCameraId")]
        public virtual Camera PrimaryCamera { get; set; }

        public int SecondaryCameraId { get; set; }

        [ForeignKey("SecondaryCameraId")]
        public virtual Camera SecondaryCamera { get; set; }

        public virtual ICollection<Lens> Lenses { get; set; }

        public virtual ICollection<Accessory> Accessories { get; set; }

        public virtual ICollection<Workshop> WorkshopsTrained { get; set; }

        public virtual ICollection<Workshop> WorkshopsParticipated { get; set; }
    }
}
