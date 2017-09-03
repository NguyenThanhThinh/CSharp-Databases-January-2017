namespace Performance.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Project
    {
        public int ProjectID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}