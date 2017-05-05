namespace Gringotts
{
    using System.Data.Entity;
    using Migrations;
    using Models;

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