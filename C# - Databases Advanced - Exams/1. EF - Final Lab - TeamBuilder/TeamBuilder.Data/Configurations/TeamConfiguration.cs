namespace TeamBuilder.Data.Configurations
{
    using Models;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    class TeamConfiguration : EntityTypeConfiguration<Team>
    {
        public TeamConfiguration()
        {
            this.Property(team => team.Name)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Teams_Name", 1) {IsUnique = true}))
                .HasMaxLength(25)
                .IsRequired();

            this.Property(team => team.Description)
                .HasMaxLength(32);

            this.Property(team => team.Acronym)
                .HasMaxLength(3)
                .IsFixedLength()
                .IsRequired();

            this.HasMany(team => team.SentInvitations)
                .WithRequired(invitation => invitation.Team)
                .WillCascadeOnDelete(false);
        }
    }
}
