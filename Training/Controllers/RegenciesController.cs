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
    public class RegenciesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Regencies
        public async Task<ActionResult> Index()
        {
            var regencies = await db.Regencies.Include("Province").ToListAsync();
            return View(regencies);
        }

        // GET: Regencies/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regency regency = await db.Regencies.FindAsync(id);
            if (regency == null)
            {
                return HttpNotFound();
            }
            return View(regency);
        }

        // GET: Regencies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Regencies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Regency regency)
        {
            if (ModelState.IsValid)
            {
                db.Regencies.Add(regency);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(regency);
        }

        // GET: Regencies/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regency regency = await db.Regencies.FindAsync(id);
            if (regency == null)
            {
                return HttpNotFound();
            }
            return View(regency);
        }

        // POST: Regencies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Regency regency)
        {
            if (ModelState.IsValid)
            {
                db.Entry(regency).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(regency);
        }

        // GET: Regencies/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Regency regency = await db.Regencies.FindAsync(id);
            if (regency == null)
            {
                return HttpNotFound();
            }
            return View(regency);
        }

        // POST: Regencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Regency regency = await db.Regencies.FindAsync(id);
            db.Regencies.Remove(regency);
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
