namespace Gringotts.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddedModelValidations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WizzardDeposits", "DepositCharge", c => c.Double(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.WizzardDeposits", "DepositCharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}