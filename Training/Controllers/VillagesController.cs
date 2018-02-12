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
using Training.ViewModels;
using System.Diagnostics;

namespace Training.Controllers
{
    public class VillagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Villages
        public async Task<ActionResult> Index()
        {
            //var villages = await db.Villages.Include("District").Include("District.Regency").Include("District.Regency.Province").ToListAsync();
            //return View(villages);
            return View();
        }

        public async Task<ActionResult> Data(ViewModels.DataTableRequest dt)
        {
            try
            {
                var search = dt.search.value;
                var totalData = await db.Villages.CountAsync();
                if (search != null)
                {
                    var villageFilterCount = await db.Villages.Include("District").Include("District.Regency")
                        .Include("District.Regency.Province")
                        .Where(x => x.Name.ToLower().Contains(search) || x.District.Name.ToLower().Contains(search)
                        || x.District.Regency.Name.ToLower().Contains(search) || x.District.Regency.Province.Name.ToLower().Contains(search))
                        .CountAsync();

                    var villageFilterData = await db.Villages.Include("District").Include("District.Regency")
                        .Include("District.Regency.Province")
                        .Where(x => x.Name.ToLower().Contains(search) || x.District.Name.ToLower().Contains(search)
                        || x.District.Regency.Name.ToLower().Contains(search) || x.District.Regency.Province.Name.ToLower().Contains(search))
                        .OrderBy(x => x.Id).Skip(dt.start).Take(dt.length).ToListAsync();

                    var villageDisplay = villageFilterData.Select(x => new ViewModels.Village()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        District = x.District.Name,
                        Regency = x.District.Regency.Name,
                        Province = x.District.Regency.Province.Name
                    });
                    var villageResultFilter = new VillageData()
                    {
                        data = villageDisplay.ToList(),
                        draw = dt.draw,
                        recordsFiltered = villageFilterCount,
                        recordsTotal = totalData
                    };
                    return Json(villageResultFilter, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var villages = await db.Villages.Include("District").Include("District.Regency")
                        .Include("District.Regency.Province")
                        .OrderBy(x => x.Id).Skip(dt.start).Take(dt.length).ToListAsync();
                    var villageDisplay = villages.Select(x => new ViewModels.Village()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        District = x.District.Name,
                        Regency = x.District.Regency.Name,
                        Province = x.District.Regency.Province.Name
                    });
                    var villageResultNoFilter = new VillageData()
                    {
                        data = villageDisplay.ToList(),
                        draw = dt.draw,
                        recordsFiltered = totalData,
                        recordsTotal = totalData
                    };
                    return Json(villageResultNoFilter, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                Trace.TraceError(ex.Message);
                Trace.TraceError(ex.StackTrace);
                return null;
            }
        }

        // GET: Villages/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Village village = await db.Villages.FindAsync(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // GET: Villages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Villages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Models.Village village)
        {
            if (ModelState.IsValid)
            {
                db.Villages.Add(village);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(village);
        }

        // GET: Villages/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Village village = await db.Villages.FindAsync(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Models.Village village)
        {
            if (ModelState.IsValid)
            {
                db.Entry(village).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(village);
        }

        // GET: Villages/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Village village = await db.Villages.FindAsync(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Models.Village village = await db.Villages.FindAsync(id);
            db.Villages.Remove(village);
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
