namespace PhotographyWorkshop.Data
{
    using PhotographyWorkshops.Models;
    using System.Data.Entity;

    public class PhotographyWorkshopContext : DbContext
    {
        public PhotographyWorkshopContext()
            : base("name=PhotographyWorkshopContext")
        {
        }

        public virtual DbSet<Lens> Lenses { get; set; }
        public virtual DbSet<Camera> Cameras { get; set; }
        public virtual DbSet<Workshop> Workshops { get; set; }
        public virtual DbSet<Accessory> Accessories { get; set; }
        public virtual DbSet<Photographer> Photographers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workshop>()
                .HasMany(workshop => workshop.Participants)
                .WithMany(photographer => photographer.WorkshopsParticipated)
                .Map(config =>
                {
                    config.MapLeftKey("WorkshopId");
                    config.MapRightKey("PhotographerId");
                    config.ToTable("WorkshopPhotographers");
                });

            modelBuilder.Entity<Workshop>()
                .HasRequired(workshop => workshop.Trainer)
                .WithMany(photographer => photographer.WorkshopsTrained)
                .HasForeignKey(workshop => workshop.TrainerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photographer>()
              .HasRequired(photographer => photographer.PrimaryCamera)
              .WithMany(cameras => cameras.PrimaryCamerasPhotographers)
              .HasForeignKey(photographer => photographer.PrimaryCameraId)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Photographer>()
                .HasRequired(p => p.SecondaryCamera)
                .WithMany(c => c.SecondaryCamerasPhotographers)
                .HasForeignKey(p => p.SecondaryCameraId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}