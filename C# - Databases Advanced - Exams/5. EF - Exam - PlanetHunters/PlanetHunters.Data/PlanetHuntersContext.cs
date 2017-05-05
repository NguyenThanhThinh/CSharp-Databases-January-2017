namespace PlanetHunters.Data
{
    using Models;
    using System.Data.Entity;

    public class PlanetHuntersContext : DbContext
    {
        public PlanetHuntersContext()
            : base("name=PlanetHuntersContext")
        {
        }

        public virtual DbSet<Star> Stars { get; set; }
        public virtual DbSet<Planet> Planets { get; set; }
        public virtual DbSet<Telescope> Telescopes { get; set; }
        public virtual DbSet<Discovery> Discoveries { get; set; }
        public virtual DbSet<Astronomer> Astronomers { get; set; }
        public virtual DbSet<StarSystem> StarSystems { get; set; }
        public virtual DbSet<Publication> Publications { get; set; }
        public virtual DbSet<Journal> Journals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Discovery>().Property(d => d.DateMade).HasColumnType("date");
            
            modelBuilder.Entity<Astronomer>()
                .HasMany(astronomer => astronomer.DiscoveriesMade)
                .WithMany(discovery => discovery.Pioneers)
                .Map(config =>
                {
                    config.MapLeftKey("AstronomerId");
                    config.MapRightKey("DiscoveryId");
                    config.ToTable("AstronomerDiscoveriesMade");
                });

            modelBuilder.Entity<Astronomer>()
                .HasMany(astronomer => astronomer.DiscoveriesObserved)
                .WithMany(discovery => discovery.Observers)
                .Map(config =>
                {
                    config.MapLeftKey("AstronomerId");
                    config.MapRightKey("DiscoveryId");
                    config.ToTable("AstronomerDiscoveriesObserved");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}