using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Training.Models;

namespace Training.Controllers
{
    [Authorize(Roles = "Editor,Author")]
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Posts
        public async Task<ActionResult> Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Posts/Details/5
        public async Task<ActionResult> Details(string permalink)
        {
            if (permalink == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = await db.Posts.Include("Categories").Include("CreatedBy").Include("UpdatedBy").Include("PublishedBy")
                .Where(x => x.Permalink == permalink)
                .SingleOrDefaultAsync();
            if (post == null)
            {
                return HttpNotFound();
            }
            post.Viewer += 1;
            db.Entry(post).State = EntityState.Modified;
            var result = await db.SaveChangesAsync();
            return View(post);
        }

        // GET: Posts/Create
        public async Task<ActionResult> Create()
        {
            var categories = await db.PostCategories
                .Where(x => x.IsDeleted == false && x.Status == Status.Published)
                .Select(i => new SelectListItem()
                {
                    Text = i.Title,
                    Value = i.Permalink,
                    Selected = false
                }).ToArrayAsync();
            ViewBag.Categories = categories;
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ViewModels.Post _post)
        {
            if (ModelState.IsValid)
            {
                //TO:DO
                //dapatkan daftar category dari string of category 
                var listCategories = new List<PostCategory>();
                foreach (var item in _post.Categories)
                {
                    var categoryItem = await db.PostCategories.FindAsync(item);
                    if(categoryItem != null)
                        listCategories.Add(categoryItem);
                }
                var currentTime = DateTimeOffset.UtcNow;
                var currentUser = await db.Users.Where(x => x.UserName == User.Identity.Name)
                    .SingleOrDefaultAsync();

                //buat object dari model post 
                var post = new Post()
                {
                    Permalink = await GeneratePermalink(_post.Title),
                    FeaturedImage = Helpers.FileHelper.UploadImage(_post.FeaturedImage),
                    Title = _post.Title,
                    Intro = _post.Intro,
                    Content = _post.Content,
                    Categories = listCategories,
                    SEOTitle = _post.SEOTitle,
                    SEODescription = _post.SEODescription,
                    SEOKeywords = _post.SEOKeywords,
                    Status = Status.Draft,
                    Created = currentTime,
                    Updated = currentTime,
                    Published = currentTime,
                    CreatedBy = currentUser,
                    UpdatedBy = currentUser,
                    PublishedBy = currentUser
                };

                //simpan ke database 
                db.Posts.Add(post);
                var result = await db.SaveChangesAsync();
                if (result > 0)
                    return RedirectToAction("Index");
                
                
            }
            var categories = await db.PostCategories
                .Where(x => x.IsDeleted == false && x.Status == Status.Published)
                .Select(i => new SelectListItem()
                {
                    Text = i.Title,
                    Value = i.Permalink,
                    Selected = false
                }).ToArrayAsync();
            ViewBag.Categories = categories;
            return View(_post);
        }
        public async Task<string> GeneratePermalink(string title)
        {
            var temporaryPermalink = title.Replace(" ", "-").Replace(",", "-").Replace(".", "-").ToLower();
            var posts = await db.Posts.Where(x => x.Title == title)
                .ToListAsync();
            if (posts != null)
            {
                var samePermalink = posts.Find(x => x.Permalink == temporaryPermalink);
                if (samePermalink == null)
                    return temporaryPermalink;
                else
                {
                    var notFound = true;
                    var counter = 1;
                    while(notFound)
                    {
                        temporaryPermalink = title.Replace(" ", "-").ToLower() + "-" + counter;
                        var checkPermalink = posts.Find(x => x.Permalink == temporaryPermalink);
                        if(checkPermalink == null)
                        {
                            notFound = false;
                        }
                        counter++;
                    }
                    return temporaryPermalink;
                }
            }
            else
            {
                return temporaryPermalink;
            }
        }
        // GET: Posts/Edit/5
        public async Task<ActionResult> Edit(string permalink)
        {
            if (permalink == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var post = await db.Posts.Include("Categories").Where(x => x.Permalink == permalink)
                .SingleOrDefaultAsync();

            if (post == null)
            {
                return HttpNotFound();
            }
            var categories = await db.PostCategories
                .Where(x => x.IsDeleted == false && x.Status == Status.Published)
                .Select(i => new SelectListItem()
                {
                    Text = i.Title,
                    Value = i.Permalink,
                    Selected = false
                }).ToArrayAsync();
            foreach(var item in categories)
            {
                var isContains = post.Categories.Any(x => x.Permalink == item.Value);
                if (isContains)
                    item.Selected = true;
            }
            ViewBag.Categories = categories;
            var postVm = new ViewModels.Post(post);
            return View(postVm);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ViewModels.Post _post)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var listCategories = new List<PostCategory>();
                    foreach (var item in _post.Categories)
                    {
                        var categoryItem = await db.PostCategories.FindAsync(item);
                        listCategories.Add(categoryItem);
                    }
                    var currentUser = await db.Users.Where(x => x.UserName == User.Identity.Name)
                        .SingleOrDefaultAsync();
                    var currentTime = DateTimeOffset.UtcNow;
                    var post = await db.Posts.Include("Categories")
                        .Where(x => x.Permalink == _post.Permalink).SingleOrDefaultAsync();
                    post.Title = _post.Title;
                    post.Content = _post.Content;
                    post.Intro = _post.Intro;
                    post.Categories = listCategories;
                    post.SEOTitle = _post.SEOTitle;
                    post.SEODescription = _post.SEODescription;
                    post.SEOKeywords = _post.SEOKeywords;
                    post.Updated = currentTime;
                    post.UpdatedBy = currentUser;

                    db.Entry(post).State = EntityState.Modified;
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                        return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Trace.TraceError(ex.StackTrace);
                }
            }
            //If model states is invalid or exception happen redisplay the form
            var categories = await db.PostCategories
                .Where(x => x.IsDeleted == false && x.Status == Status.Published)
                .Select(i => new SelectListItem()
                {
                    Text = i.Title,
                    Value = i.Permalink,
                    Selected = false
                }).ToArrayAsync();
            foreach (var item in categories)
            {
                var isContains = _post.Categories.Any(x => x == item.Value);
                if (isContains)
                    item.Selected = true;
            }
            ViewBag.Categories = categories;
            return View(_post);
        }

        // GET: Posts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
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
