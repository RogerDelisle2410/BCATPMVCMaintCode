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
    public class DewlinedatasController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: dewlinedatas
        public ActionResult Index(string searchString)
        {
            var dewlinedata = from s in db.dewlines
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                dewlinedata = dewlinedata.Where(s => s.name.Contains(searchString));
            }
            return View(dewlinedata.ToList());
        }

        // GET: dewlinedatas/Details/5
        public ActionResult Details(int? id, string value)
        {
            dewline dewline = db.dewlines.Find(id);
            if (dewline == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.dewlines)
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
                dewline = db.dewlines.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                dewline = db.dewlines.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                dewline = db.dewlines.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                dewline = db.dewlines.Find(ViewBag.last);
            }

            return View(dewline);
        }

        // GET: dewlinedatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: dewlinedatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] dewline dewlinedata)
        {
            if (ModelState.IsValid)
            {
                db.dewlines.Add(dewlinedata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dewlinedata);
        }

        // GET: dewlinedatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dewline dewlinedata = db.dewlines.Find(id);
            if (dewlinedata == null)
            {
                return HttpNotFound();
            }
            return View(dewlinedata);
        }

        // POST: dewlinedatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] dewline dewlinedata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dewlinedata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dewlinedata);
        }

        // GET: dewlinedatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dewline dewlinedata = db.dewlines.Find(id);
            if (dewlinedata == null)
            {
                return HttpNotFound();
            }
            return View(dewlinedata);
        }

        // POST: dewlinedatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            dewline dewlinedata = db.dewlines.Find(id);
            db.dewlines.Remove(dewlinedata);
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
