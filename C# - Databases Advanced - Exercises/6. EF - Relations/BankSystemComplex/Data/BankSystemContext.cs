namespace BankSystemComplex.Data
{
    using Configurations;
    using Models;
    using System.Data.Entity;

    public class BankSystemContext : DbContext
    {
        public BankSystemContext()
            : base("name=BankSystemContext")
        {
        }

        public virtual DbSet<CheckingAccount> CheckingAccounts { get; set; }

        public virtual DbSet<SavingAccount> SavingAccounts { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new CheckingAccountConfiguration());
            modelBuilder.Configurations.Add(new SavingAccountConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}