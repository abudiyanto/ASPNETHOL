namespace Training.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostV3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PostCategoryPosts", newName: "PostPostCategories");
            DropPrimaryKey("dbo.PostPostCategories");
            AddPrimaryKey("dbo.PostPostCategories", new[] { "Post_Permalink", "PostCategory_Permalink" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.PostPostCategories");
            AddPrimaryKey("dbo.PostPostCategories", new[] { "PostCategory_Permalink", "Post_Permalink" });
            RenameTable(name: "dbo.PostPostCategories", newName: "PostCategoryPosts");
        }
    }
}
