namespace ProductsShop.Data
{
    using Model;
    using System.Data.Entity;

    public class ProductsShopContext : DbContext
    {
        public ProductsShopContext()
            : base("name=ProductsShopContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(user => user.MyFriends)
                .WithMany(user => user.FriendsWithMe)
                .Map(config =>
                {
                    config.MapLeftKey("UserId");
                    config.MapRightKey("FriendId");
                    config.ToTable("UserFriends");
                });

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .Map(config =>
                {
                    config.MapLeftKey("ProductId");
                    config.MapRightKey("CategoryId");
                    config.ToTable("CategoryProducts");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}