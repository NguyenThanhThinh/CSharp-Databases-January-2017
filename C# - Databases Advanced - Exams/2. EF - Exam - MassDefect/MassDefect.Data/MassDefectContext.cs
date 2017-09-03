namespace MassDefect.Data
{
    using Models;
    using System.Data.Entity;

    public class MassDefectContext : DbContext
    {
        public MassDefectContext()
            : base("name=MassDefectContext")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<MassDefectContext>());
        }

        public virtual DbSet<Star> Stars { get; set; }
        public virtual DbSet<Planet> Planets { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Anomaly> Anomalies { get; set; }
        public virtual DbSet<SolarSystem> SolarSystems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anomaly>()
                .HasMany(anomaly => anomaly.Victims)
                .WithMany(person => person.Anomalies)
                .Map(config =>
                {
                    config.MapLeftKey("AnomalyId");
                    config.MapRightKey("PersonId");
                    config.ToTable("AnomalyVictims");
                });

            // Now all the FKs between the models are set to nullable (int?).

            // If all the FKs in the models are not set to nullable (int?) - then we have to
            // specify that on delete there will be no cascade delete for the records.
            // Otherwise we cannot create the database and exceptions are rising.

            // But also we have to specify the relation between a model and two collections of other model (with [InversePropery]) -
            // like the case with origin/teleport Anomalies and origin/teleport Planets. Because if we don't do it -
            // there will be four FKs created instead of two. However if we don't use nullable (int?) properties
            // and we work with the fluent API like below - this is not the case and the FKs will be correct.

            //modelBuilder.Entity<Anomaly>()
            //    .HasRequired(anomaly => anomaly.OriginPlanet)
            //    .WithMany(planet => planet.OriginAnomalies)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Anomaly>()
            //    .HasRequired(anomaly => anomaly.TeleportPlanet)
            //    .WithMany(planet => planet.TeleportAnomalies)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<Star>()
            //   .HasRequired(star => star.SolarSystem)
            //   .WithMany(solarSystem => solarSystem.Stars)
            //   .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}