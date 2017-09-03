namespace LocalStore
{
    using Models;
    using System.Data.Entity;

    public class LocalStoreContext : DbContext
    {
        public LocalStoreContext()
            : base("name=LocalStoreContext")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<LocalStoreContext>());
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}