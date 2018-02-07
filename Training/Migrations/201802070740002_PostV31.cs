namespace Training.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostV31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Viewer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Viewer");
        }
    }
}
