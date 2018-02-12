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
using System.Diagnostics;
using Training.ViewModels;

namespace Training.Controllers
{
    public class DistrictsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Districts
        public async Task<ActionResult> Index()
        {
            //var districts = await db.Districts.Include("Regency").Include("Regency.Province").ToListAsync();
            //return View(districts);
            return View();
        }

        public async Task<ActionResult> Data(ViewModels.DataTableRequest dt)
        {
            try
            {
                var search = dt.search.value;
                var totalData = await db.Districts.CountAsync();
                if (search != null)
                {
                    var districtFilterCount = await db.Districts.Include("Regency").Include("Regency.Province")
                        .Where(x => x.Name.ToLower().Contains(search) || x.Regency.Name.ToLower().Contains(search)
                        || x.Regency.Province.Name.ToLower().Contains(search))
                        .CountAsync();

                    var districtFilterData = await db.Districts.Include("Regency").Include("Regency.Province")
                        .Where(x => x.Name.ToLower().Contains(search) || x.Regency.Name.ToLower().Contains(search)
                        || x.Regency.Province.Name.ToLower().Contains(search))
                        .OrderBy(x => x.Id).Skip(dt.start).Take(dt.length).ToListAsync();

                    var districtDisplay = districtFilterData.Select(x => new ViewModels.District()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Regency = x.Regency.Name,
                        Province = x.Regency.Province.Name
                    });
                    var districtResultFilter = new DistrictData()
                    {
                        data = districtDisplay.ToList(),
                        draw = dt.draw,
                        recordsFiltered = districtFilterCount,
                        recordsTotal = totalData
                    };
                    return Json(districtResultFilter, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var districts = await db.Districts.Include("Regency").Include("Regency.Province")
                        .OrderBy(x => x.Id).Skip(dt.start).Take(dt.length).ToListAsync();
                    var districtDisplay = districts.Select(x => new ViewModels.District()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Regency = x.Regency.Name,
                        Province = x.Regency.Province.Name
                    });
                    var districtResultNoFilter = new DistrictData()
                    {
                        data = districtDisplay.ToList(),
                        draw = dt.draw,
                        recordsFiltered = totalData,
                        recordsTotal = totalData
                    };
                    return Json(districtResultNoFilter, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                return null;
            }
        }

        // GET: Districts/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.District district = await db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // GET: Districts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Models.District district)
        {
            if (ModelState.IsValid)
            {
                db.Districts.Add(district);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(district);
        }

        // GET: Districts/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.District district = await db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Models.District district)
        {
            if (ModelState.IsValid)
            {
                db.Entry(district).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(district);
        }

        // GET: Districts/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.District district = await db.Districts.FindAsync(id);
            if (district == null)
            {
                return HttpNotFound();
            }
            return View(district);
        }

        // POST: Districts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Models.District district = await db.Districts.FindAsync(id);
            db.Districts.Remove(district);
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
