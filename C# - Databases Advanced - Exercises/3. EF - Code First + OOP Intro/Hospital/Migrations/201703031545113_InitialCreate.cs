namespace Hospital.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diagnoses",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Comments = c.String(),
                    Patient_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id, cascadeDelete: true)
                .Index(t => t.Patient_Id);

            CreateTable(
                "dbo.Patients",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(nullable: false),
                    LastName = c.String(nullable: false),
                    Address = c.String(nullable: false),
                    Email = c.String(nullable: false),
                    DateOfBirth = c.DateTime(nullable: false),
                    Picture = c.Binary(),
                    HasMedicalInsuranse = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Medicaments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Visitations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Comments = c.String(),
                    Patient_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.Patient_Id, cascadeDelete: true)
                .Index(t => t.Patient_Id);

            CreateTable(
                "dbo.MedicamentPatients",
                c => new
                {
                    Medicament_Id = c.Int(nullable: false),
                    Patient_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Medicament_Id, t.Patient_Id })
                .ForeignKey("dbo.Medicaments", t => t.Medicament_Id, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_Id, cascadeDelete: true)
                .Index(t => t.Medicament_Id)
                .Index(t => t.Patient_Id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Diagnoses", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.Visitations", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.MedicamentPatients", "Patient_Id", "dbo.Patients");
            DropForeignKey("dbo.MedicamentPatients", "Medicament_Id", "dbo.Medicaments");
            DropIndex("dbo.MedicamentPatients", new[] { "Patient_Id" });
            DropIndex("dbo.MedicamentPatients", new[] { "Medicament_Id" });
            DropIndex("dbo.Visitations", new[] { "Patient_Id" });
            DropIndex("dbo.Diagnoses", new[] { "Patient_Id" });
            DropTable("dbo.MedicamentPatients");
            DropTable("dbo.Visitations");
            DropTable("dbo.Medicaments");
            DropTable("dbo.Patients");
            DropTable("dbo.Diagnoses");
        }
    }
}