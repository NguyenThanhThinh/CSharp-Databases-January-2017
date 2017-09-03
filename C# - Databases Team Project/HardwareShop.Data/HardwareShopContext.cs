namespace HardwareShop.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Migrations;
    using Models.EntityModels;
    using System.Data.Entity;

    public class HardwareShopContext : IdentityDbContext<ApplicationUser>
    {
        public HardwareShopContext()
            : base(@"data source=(LocalDb)\MSSQLLocalDB;initial catalog=HardwareShop;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<HardwareShopContext, Configuration>());
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        public virtual DbSet<Review> Reviews { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<SubCategory> SubCategories { get; set; }

        public static HardwareShopContext Create()
        {
            return new HardwareShopContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(item => item.Carts)
                .WithMany(cart => cart.Items)
                .Map(cfg =>
                {
                    cfg.MapLeftKey("ItemId");
                    cfg.MapRightKey("CartId");
                    cfg.ToTable("ItemCarts");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}