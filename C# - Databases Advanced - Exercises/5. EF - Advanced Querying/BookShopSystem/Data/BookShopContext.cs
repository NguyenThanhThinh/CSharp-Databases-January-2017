namespace BookShopSystem.Data
{
    using Models;
    using Migrations;
    using System.Data.Entity;

    public class BookShopContext : DbContext
    {
        public BookShopContext()
            : base("name=BookShopContext")
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<BookShopContext, Configuration>());
        }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasMany(x => x.RelatedBooks)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("RelatedBooks");
                    m.MapLeftKey("BookId");
                    m.MapRightKey("RelatedBookId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}