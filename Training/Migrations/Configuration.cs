namespace Training.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity.Migrations;
    using Training.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Training.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Training.Models.ApplicationDbContext";
        }
        protected override void Seed(Training.Models.ApplicationDbContext context)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var roleAuthor = roleManager.FindByName("Author");
            if(roleAuthor == null)
            {
                roleManager.Create(new IdentityRole { Name = "Author" });
            }
            var roleEditor = roleManager.FindByName("Editor");
            if (roleEditor == null)
            {
                roleManager.Create(new IdentityRole { Name = "Editor" });
            }
            var author = new ApplicationUser
            {
                PhoneNumber = "+6282311028592",
                PhoneNumberConfirmed = true,
                UserName = "author2@training.id",
                Email = "author2@training.id"
            };
            if (manager.FindByName("author2@training.id") == null)
            {
                manager.Create(author, "Training@2018");
                manager.AddToRoles(author.Id, new string[] { "Author" });
            }
            var editor = new ApplicationUser
            {
                PhoneNumber = "+6282311028592",
                PhoneNumberConfirmed = true,
                UserName = "editor@training.id",
                Email = "editor@training.id"
            };
            if (manager.FindByName("editor@training.id") == null)
            {
                manager.Create(editor, "Training@2018");
                manager.AddToRoles(editor.Id, new string[] { "Editor" });
            }
        }
    }
}
