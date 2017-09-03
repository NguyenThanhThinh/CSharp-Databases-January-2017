namespace BankSystemComplex.Data.Configurations
{
    using Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;

    public class CheckingAccountConfiguration : EntityTypeConfiguration<CheckingAccount>
    {
        public CheckingAccountConfiguration()
        {
            // Set primary key.
            this.HasKey(a => a.Id);

            // Set required properties.
            this.Property(a => a.AccountNumber).IsRequired();
            this.HasRequired(a => a.User).WithMany(u => u.CheckingAccounts);

            // Set unique.
            this.Property(a => a.AccountNumber).HasColumnAnnotation("IX_SavingAccounts_AccountNumber", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
        }
    }
}