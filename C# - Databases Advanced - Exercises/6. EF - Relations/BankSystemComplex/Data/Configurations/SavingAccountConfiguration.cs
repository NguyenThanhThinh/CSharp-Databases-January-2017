namespace BankSystemComplex.Data.Configurations
{
    using Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;

    public class SavingAccountConfiguration : EntityTypeConfiguration<SavingAccount>
    {
        public SavingAccountConfiguration()
        {
            // Set primary key.
            this.HasKey(a => a.Id);

            // Set required properties.
            this.Property(a => a.AccountNumber).IsRequired();
            this.HasRequired(a => a.User).WithMany(u => u.SavingAccounts);

            // Set unique.
            this.Property(a => a.AccountNumber).HasColumnAnnotation("IX_SavingAccounts_Username", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}