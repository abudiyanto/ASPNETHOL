namespace Training.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegionsV1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Regency_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Regencies", t => t.Regency_Id)
                .Index(t => t.Regency_Id);
            
            CreateTable(
                "dbo.Regencies",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Province_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.Province_Id)
                .Index(t => t.Province_Id);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Villages",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        District_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.District_Id)
                .Index(t => t.District_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Villages", "District_Id", "dbo.Districts");
            DropForeignKey("dbo.Districts", "Regency_Id", "dbo.Regencies");
            DropForeignKey("dbo.Regencies", "Province_Id", "dbo.Provinces");
            DropIndex("dbo.Villages", new[] { "District_Id" });
            DropIndex("dbo.Regencies", new[] { "Province_Id" });
            DropIndex("dbo.Districts", new[] { "Regency_Id" });
            DropTable("dbo.Villages");
            DropTable("dbo.Provinces");
            DropTable("dbo.Regencies");
            DropTable("dbo.Districts");
        }
    }
}
