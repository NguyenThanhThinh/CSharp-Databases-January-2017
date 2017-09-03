using System.Data.Entity.Migrations;

namespace Sales.Migrations
{
    public partial class ProductsAddColumnDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Description", c => c.String(defaultValue: "No description"));
        }

        public override void Down()
        {
            DropColumn("dbo.Products", "Description");
        }
    }
}