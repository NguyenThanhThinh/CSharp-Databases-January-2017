namespace WeddingsPlanner.Data
{
    using Models;
    using System.Data.Entity;

    public class WeddingsPlannerContext : DbContext
    {
        public WeddingsPlannerContext()
            : base("name=WeddingsPlannerContext")
        {
        }

        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Agency> Agencies { get; set; }
        public virtual DbSet<Wedding> Weddings { get; set; }
        public virtual DbSet<Present> Presents { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invitation>()
                .HasRequired(invitation => invitation.Present)
                .WithRequiredPrincipal(present => present.Invitation);

            modelBuilder.Entity<Wedding>()
                .HasMany(wedding => wedding.Venues)
                .WithMany(venue => venue.Weddings)
                .Map(config =>
                {
                    config.MapLeftKey("WeddingId");
                    config.MapRightKey("VenueId");
                    config.ToTable("WeddingVenues");
                });

            modelBuilder.Entity<Wedding>()
                .HasRequired(wedding => wedding.Bride)
                .WithMany(bride => bride.BrideWeddings)
                .HasForeignKey(wedding => wedding.BrideId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
               .HasRequired(wedding => wedding.Bridegroom)
               .WithMany(bride => bride.BridegroomWeddings)
               .HasForeignKey(wedding => wedding.BridegroomId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(wedding => wedding.Invitations)
                .WithRequired(invitation => invitation.Wedding)
                .HasForeignKey(invitation => invitation.WeddingId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}