namespace LocalStore
{
    using System.Data.Entity;
    using Models;

    public class LocalStoreContext : DbContext
    {
        public LocalStoreContext()
            : base("name=LocalStoreContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LocalStoreContext>());
        }

        public virtual DbSet<Product> Products{ get; set; }
    }
}