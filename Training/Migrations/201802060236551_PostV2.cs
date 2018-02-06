namespace Training.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostV2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        Permalink = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Order = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Updated = c.DateTimeOffset(nullable: false, precision: 7),
                        Deleted = c.DateTimeOffset(nullable: false, precision: 7),
                        Published = c.DateTimeOffset(nullable: false, precision: 7),
                        Status = c.Int(nullable: false),
                        CreatedBy_Id = c.String(maxLength: 128),
                        DeletedBy_Id = c.String(maxLength: 128),
                        PublishedBy_Id = c.String(maxLength: 128),
                        UpdatedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Permalink)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.DeletedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PublishedBy_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy_Id)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.DeletedBy_Id)
                .Index(t => t.PublishedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "dbo.PostCategoryPosts",
                c => new
                    {
                        PostCategory_Permalink = c.String(nullable: false, maxLength: 128),
                        Post_Permalink = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.PostCategory_Permalink, t.Post_Permalink })
                .ForeignKey("dbo.PostCategories", t => t.PostCategory_Permalink, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Post_Permalink, cascadeDelete: true)
                .Index(t => t.PostCategory_Permalink)
                .Index(t => t.Post_Permalink);
            
            AddColumn("dbo.Posts", "FeaturedImage", c => c.String());
            AddColumn("dbo.Posts", "IsFeatured", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "SEOTitle", c => c.String());
            AddColumn("dbo.Posts", "SEODescription", c => c.String());
            AddColumn("dbo.Posts", "SEOKeywords", c => c.String());
            AddColumn("dbo.Posts", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "Updated", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Posts", "Deleted", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Posts", "CreatedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "DeletedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "PublishedBy_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Posts", "UpdatedBy_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Posts", "CreatedBy_Id");
            CreateIndex("dbo.Posts", "DeletedBy_Id");
            CreateIndex("dbo.Posts", "PublishedBy_Id");
            CreateIndex("dbo.Posts", "UpdatedBy_Id");
            AddForeignKey("dbo.Posts", "CreatedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "DeletedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "PublishedBy_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Posts", "UpdatedBy_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "PublishedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "DeletedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Posts", "CreatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostCategories", "UpdatedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostCategories", "PublishedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostCategoryPosts", "Post_Permalink", "dbo.Posts");
            DropForeignKey("dbo.PostCategoryPosts", "PostCategory_Permalink", "dbo.PostCategories");
            DropForeignKey("dbo.PostCategories", "DeletedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostCategories", "CreatedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.PostCategoryPosts", new[] { "Post_Permalink" });
            DropIndex("dbo.PostCategoryPosts", new[] { "PostCategory_Permalink" });
            DropIndex("dbo.PostCategories", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.PostCategories", new[] { "PublishedBy_Id" });
            DropIndex("dbo.PostCategories", new[] { "DeletedBy_Id" });
            DropIndex("dbo.PostCategories", new[] { "CreatedBy_Id" });
            DropIndex("dbo.Posts", new[] { "UpdatedBy_Id" });
            DropIndex("dbo.Posts", new[] { "PublishedBy_Id" });
            DropIndex("dbo.Posts", new[] { "DeletedBy_Id" });
            DropIndex("dbo.Posts", new[] { "CreatedBy_Id" });
            DropColumn("dbo.Posts", "UpdatedBy_Id");
            DropColumn("dbo.Posts", "PublishedBy_Id");
            DropColumn("dbo.Posts", "DeletedBy_Id");
            DropColumn("dbo.Posts", "CreatedBy_Id");
            DropColumn("dbo.Posts", "Deleted");
            DropColumn("dbo.Posts", "Updated");
            DropColumn("dbo.Posts", "IsDeleted");
            DropColumn("dbo.Posts", "SEOKeywords");
            DropColumn("dbo.Posts", "SEODescription");
            DropColumn("dbo.Posts", "SEOTitle");
            DropColumn("dbo.Posts", "IsFeatured");
            DropColumn("dbo.Posts", "FeaturedImage");
            DropTable("dbo.PostCategoryPosts");
            DropTable("dbo.PostCategories");
        }
    }
}
