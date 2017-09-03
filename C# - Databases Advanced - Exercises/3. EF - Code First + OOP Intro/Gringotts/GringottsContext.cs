namespace Gringotts
{
    using Migrations;
    using Models;
    using System.Data.Entity;

    public class GringottsContext : DbContext
    {
        public GringottsContext()
            : base("name=GringottsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GringottsContext, Configuration>());
        }

        public virtual DbSet<WizzardDeposit> WizzardDeposits { get; set; }
    }
}