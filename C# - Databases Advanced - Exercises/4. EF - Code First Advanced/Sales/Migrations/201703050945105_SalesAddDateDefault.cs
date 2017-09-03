using System.Data.Entity.Migrations;

namespace Sales.Migrations
{
    public partial class SalesAddDateDefault : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sales", "Date", s => s.String(defaultValueSql: "GETDATE()"));
        }

        public override void Down()
        {
            AlterColumn("dbo.Sale", "Date", s => s.String(defaultValueSql: "NULL"));
        }
    }
}