namespace CarDealer.Data
{
    using Models;
    using System.Data.Entity;

    public class CarDealerContext : DbContext
    {
        public CarDealerContext()
            : base("name=CarDealerContext")
        {
            Configuration.LazyLoadingEnabled = false;
            //Database.SetInitializer(new DropCreateDatabaseAlways<CarDealerContext>());
        }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Part> Parts { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Part>()
                .HasMany(part => part.Cars)
                .WithMany(car => car.Parts)
                .Map(config =>
                {
                    config.MapLeftKey("PartId");
                    config.MapRightKey("CarId");
                    config.ToTable("PartCars");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}