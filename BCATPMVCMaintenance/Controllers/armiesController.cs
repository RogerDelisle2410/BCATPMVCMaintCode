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
    public class ArmiesController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: armies
        public ActionResult Index(string searchString)
        {
            var army = from s in db.armies
                             select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                army = army.Where(s => s.name.Contains(searchString));
            }
            return View(army.ToList());
        }

        // GET: armies/Details/5
        public ActionResult Details(int? id, string value)
        {
            army army = db.armies.Find(id);
            if (army == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.navies)
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
                army = db.armies.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                army = db.armies.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                army = db.armies.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                army = db.armies.Find(ViewBag.last);
            }

            return View(army);
        }

        // GET: armies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: armies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] army army)
        {
            if (ModelState.IsValid)
            {
                db.armies.Add(army);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(army);
        }

        // GET: armies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            army army = db.armies.Find(id);
            if (army == null)
            {
                return HttpNotFound();
            }
            return View(army);
        }

        // POST: armies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] army army)
        {
            if (ModelState.IsValid)
            {
                db.Entry(army).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(army);
        }

        // GET: armies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            army army = db.armies.Find(id);
            if (army == null)
            {
                return HttpNotFound();
            }
            return View(army);
        }

        // POST: armies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            army army = db.armies.Find(id);
            db.armies.Remove(army);
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
