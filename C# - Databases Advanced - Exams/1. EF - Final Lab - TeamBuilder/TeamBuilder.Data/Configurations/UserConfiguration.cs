namespace TeamBuilder.Data.Configurations
{
    using Models;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.Property(user => user.Username)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Users_Username", 1) {IsUnique = true}))
                .HasMaxLength(25)
                .IsRequired();

            this.Property(user => user.FirstName)
                .HasMaxLength(25);

            this.Property(user => user.LastName)
                .HasMaxLength(25);

            this.Property(user => user.Password)
                .HasMaxLength(30)
                .IsRequired();

            this.HasMany(user => user.CreatedTeams)
                .WithRequired(team => team.Creator)
                .WillCascadeOnDelete(false);

            this.HasMany(user => user.CreatedEvents)
                .WithRequired(createdEvent => createdEvent.Creator)
                .WillCascadeOnDelete(false);

            this.HasMany(user => user.ReceivedInvitations)
                .WithRequired(invitation => invitation.InvitedUser)
                .WillCascadeOnDelete(false);

            this.HasMany(user => user.Teams)
                .WithMany(team => team.Members)
                .Map(config =>
                {
                    config.MapLeftKey("UserId");
                    config.MapRightKey("TeamId");
                    config.ToTable("UserTeams");
                });
        }
    }
}
