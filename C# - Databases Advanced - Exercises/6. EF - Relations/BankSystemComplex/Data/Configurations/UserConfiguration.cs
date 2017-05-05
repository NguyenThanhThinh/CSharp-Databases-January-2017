namespace BankSystemComplex.Data.Configurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Models;

    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            // Set primary key.
            this.HasKey(u => u.Id);

            // Set fields as required.
            this.Property(u => u.Username).IsRequired();
            this.Property(u => u.Email).IsRequired();
            this.Property(u => u.Password).IsRequired();

            // Set fields as unique.
            this.Property(u => u.Username).HasColumnAnnotation("IX_Users_Username", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
            this.Property(u => u.Email).HasColumnAnnotation("IX_Users_Email", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}
