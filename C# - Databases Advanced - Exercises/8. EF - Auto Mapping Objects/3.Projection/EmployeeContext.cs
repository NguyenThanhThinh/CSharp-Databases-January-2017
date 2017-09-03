namespace _3.Projection
{
    using Models;
    using System.Data.Entity;

    public class EmployeeContext : DbContext
    {
        public EmployeeContext()
            : base("name=EmployeeContext")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<EmployeeContext>());
        }

        public DbSet<Employee> Employees { get; set; }
    }
}