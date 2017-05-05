namespace ProductsShop.Data
{
    using Models;
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
                .HasMany(user => user.Friends)
                .WithMany()
                .Map(config =>
                {
                    config.MapLeftKey("UserId");
                    config.MapRightKey("FriendId");
                    config.ToTable("UserFriends");
                });

            // If we don't want to work with annotations [InverseProperty("")] for buyer/seller bought/sold products
            // we can use the modelBuilder which is working with the navigational property of the FK;
            // We can omit the FK specification because the EF will understand it if it is named accordingly;
            
            //modelBuilder.Entity<User>()
            //    .HasMany(user => user.ProductsBought)
            //    .WithOptional(product => product.Buyer)
            //    .HasForeignKey(product => product.BuyerId);
                
            //modelBuilder.Entity<User>()
            //    .HasMany(user => user.ProductsBought)
            //    .WithOptional(product => product.Seller)
            //    .HasForeignKey(product => product.SellerId);
            
            modelBuilder.Entity<Category>()
                .HasMany(category => category.Products)
                .WithMany(product => product.Categories)
                .Map(config =>
                {
                    config.MapLeftKey("CategoryId");
                    config.MapRightKey("ProductId");
                    config.ToTable("CategoryProducts");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}