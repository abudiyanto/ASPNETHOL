namespace Training.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Post : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Permalink = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Content = c.String(),
                        Intro = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Published = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Permalink);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Posts");
        }
    }
}
