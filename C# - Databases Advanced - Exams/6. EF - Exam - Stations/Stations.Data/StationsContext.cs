namespace Stations.Data
{
    using Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Annotations;

    public class StationsContext : DbContext
    {
        public StationsContext()
             : base("name=StationsContext")
        {
        }

        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Train> Trains { get; set; }
        public virtual DbSet<TrainSeat> TrainSeats { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<SeatingClass> SeatingClasses { get; set; }
        public virtual DbSet<CustomerCard> CustomerCards { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>()
                     .Property(s => s.Name)
                     .HasColumnAnnotation(
                         IndexAnnotation.AnnotationName,
                         new IndexAnnotation(
                             new IndexAttribute("IX_Station_Name", 1) { IsUnique = true }));

            modelBuilder.Entity<Train>()
                .Property(s => s.TrainNumber)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_Train_TrainNumber", 1) { IsUnique = true }));

            modelBuilder.Entity<SeatingClass>()
                .Property(s => s.Name)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_SeatingClass_Name", 1) { IsUnique = true }));

            modelBuilder.Entity<SeatingClass>()
                .Property(s => s.Abbreviation)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_SeatingClass_Abbreviation", 1) { IsUnique = true }));

            modelBuilder.Entity<Trip>()
                .HasRequired(trip => trip.OriginStation)
                .WithMany(station => station.TripsFrom)
                .HasForeignKey(trip => trip.OriginStationId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trip>()
                .HasRequired(trai => trai.DestinationStation)
                .WithMany(station => station.TripsTo)
                .HasForeignKey(train => train.DestinationStationId)
                .WillCascadeOnDelete(false);
        }
    }
}