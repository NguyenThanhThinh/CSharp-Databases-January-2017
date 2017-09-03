using System.Collections.Generic;

namespace Hospital.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Medicament
    {
        public Medicament()
        {
            this.Patients = new HashSet<Patient>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}