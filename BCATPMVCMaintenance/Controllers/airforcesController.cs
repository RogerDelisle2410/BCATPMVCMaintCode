using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BCATPMVCMaintenance.Models;

namespace BCATPMVCMaintenance.Controllers
{
    public class AirforcesController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: airforces
        public ActionResult Index(string searchString)
        {
            var airforce = from s in db.airforces
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                airforce = airforce.Where(s => s.name.Contains(searchString));
            }
            return View(airforce.ToList());
        }

        // GET: airforces/Details/5
        public ActionResult Details(int? id, string value)
        {
            airforce airforce = db.airforces.Find(id);
            if (airforce == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.airforces)
            {
                allTheSheets.Add(item.Id);
            }

            for (int i = 0; i < allTheSheets.Count; i++)
            {
                ViewBag.Current = id;
                if (i == 0)
                {
                    ViewBag.first = allTheSheets[i];
                }
                if (i == allTheSheets.Count - 1)
                {
                    ViewBag.last = allTheSheets[i];
                }
            }

            ViewBag.allKeys = allTheSheets;

            int index = ViewBag.allKeys.IndexOf((int)id);
            if (index + 1 <= ViewBag.allKeys.Count - 1 && value == "next")
            {
                airforce = db.airforces.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                airforce = db.airforces.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                airforce = db.airforces.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                airforce = db.airforces.Find(ViewBag.last);
            }

            return View(airforce);
        }

        // GET: airforces/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: airforces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] airforce airforce)
        {
            if (ModelState.IsValid)
            {
                db.airforces.Add(airforce);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(airforce);
        }

        // GET: airforces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            airforce airforce = db.airforces.Find(id);
            if (airforce == null)
            {
                return HttpNotFound();
            }
            return View(airforce);
        }

        // POST: airforces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] airforce airforce)
        {
            if (ModelState.IsValid)
            {
                db.Entry(airforce).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(airforce);
        }

        // GET: airforces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            airforce airforce = db.airforces.Find(id);
            if (airforce == null)
            {
                return HttpNotFound();
            }
            return View(airforce);
        }

        // POST: airforces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            airforce airforce = db.airforces.Find(id);
            db.airforces.Remove(airforce);
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
