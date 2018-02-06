using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Training.Models;

namespace Training.Controllers
{
    [Authorize(Roles = "Editor,Author")]
    public class PostCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PostCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.PostCategories.ToListAsync());
        }

        // GET: PostCategories/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCategory postCategory = await db.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            return View(postCategory);
        }

        // GET: PostCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PostCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModels.PostCategory _category)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await db.Users.Where(x => x.UserName == User.Identity.Name)
                    .SingleOrDefaultAsync();
                if(currentUser != null)
                {
                    var currentTime = DateTimeOffset.UtcNow;
                    var category = new PostCategory()
                    {
                        Permalink = _category.Title.Replace(" ", "-").ToLower(),
                        Title = _category.Title,
                        Order = _category.Order,
                        Status = Status.Published,
                        Created = currentTime,
                        Updated = currentTime,
                        CreatedBy = currentUser,
                        UpdatedBy = currentUser,
                        IsDeleted = false,
                        Published = currentTime,
                        PublishedBy = currentUser
                    };
                    db.PostCategories.Add(category);
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                        return RedirectToAction("Index");
                }
            }
            return View(_category);
        }

        // GET: PostCategories/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var postCategory = await db.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            //var postCategoryVm = new ViewModels.PostCategory()
            //{
            //    Order = postCategory.Order,
            //    Permalink = postCategory.Permalink,
            //    Title = postCategory.Title
            //};
            var postCategoryVm = new ViewModels.PostCategory(postCategory);
            return View(postCategoryVm);
        }

        // POST: PostCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ViewModels.PostCategory _category)
        {
            if (ModelState.IsValid)
            {
                if(_category.Permalink != null)
                {
                    var postCategory = await db.PostCategories.FindAsync(_category.Permalink);
                    if(postCategory != null)
                    {
                        postCategory.Title = _category.Title;
                        postCategory.Order = _category.Order;
                        db.Entry(postCategory).State = EntityState.Modified;
                        var result = await db.SaveChangesAsync();
                        if(result > 0)
                            return RedirectToAction("Index");
                    }
                }
            }
            return View(_category);
        }

        // GET: PostCategories/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostCategory postCategory = await db.PostCategories.FindAsync(id);
            if (postCategory == null)
            {
                return HttpNotFound();
            }
            return View(postCategory);
        }

        // POST: PostCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            PostCategory postCategory = await db.PostCategories.FindAsync(id);
            db.PostCategories.Remove(postCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
