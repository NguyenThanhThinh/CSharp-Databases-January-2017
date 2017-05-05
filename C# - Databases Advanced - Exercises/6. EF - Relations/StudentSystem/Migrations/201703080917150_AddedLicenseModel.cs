namespace StudentSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLicenseModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Licenses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Resource_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.Resource_Id, cascadeDelete: true)
                .Index(t => t.Resource_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Licenses", "Resource_Id", "dbo.Resources");
            DropIndex("dbo.Licenses", new[] { "Resource_Id" });
            DropTable("dbo.Licenses");
        }
    }
}
