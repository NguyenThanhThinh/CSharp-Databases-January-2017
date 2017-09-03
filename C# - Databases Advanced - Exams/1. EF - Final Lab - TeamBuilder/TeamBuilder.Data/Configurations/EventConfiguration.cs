namespace TeamBuilder.Data.Configurations
{
    using Models;
    using System.Data.Entity.ModelConfiguration;

    internal class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration()
        {
            this.Property(createdEvent => createdEvent.Name)
                .HasMaxLength(25)
                .IsRequired();

            this.Property(createdEvent => createdEvent.Description)
                .HasMaxLength(250);

            this.HasRequired(createdEvent => createdEvent.Creator)
                .WithMany(user => user.CreatedEvents);

            this.HasMany(createdEvent => createdEvent.ParticipatingTeams)
                .WithMany(team => team.ParticipatedEvents)
                .Map(config =>
                {
                    config.MapLeftKey("EventId");
                    config.MapRightKey("TeamId");
                    config.ToTable("EventTeams");
                });
        }
    }
}