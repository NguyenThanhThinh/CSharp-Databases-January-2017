namespace StudentSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public class Course
    {
        public Course()
        {
            this.Students = new HashSet<Student>();
            this.Resources = new HashSet<Resource>();
            this.Homeworks = new HashSet<Homework>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }

        public virtual ICollection<Homework> Homeworks { get; set; }
    }
}
