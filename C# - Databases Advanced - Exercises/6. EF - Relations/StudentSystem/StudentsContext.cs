namespace StudentSystem
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class StudentsContext : DbContext
    {
        public StudentsContext()
            : base("name=StudentsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<StudentsContext, Configuration>());
        }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Resource> Resources { get; set; }

        public virtual DbSet<Homework> Homeworks { get; set; }

        public virtual DbSet<License> Licenses { get; set; }
    }
}