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
    public class DefunctbasController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: defunctbas
        public ActionResult Index(string searchString)
        {
            var defunctbase = from s in db.defuncts
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                defunctbase = defunctbase.Where(s => s.name.Contains(searchString));
            }
            return View(defunctbase.ToList());
        }

        // GET: defunctbas/Details/5
        public ActionResult Details(int? id, string value)
        {
            defunct defunct = db.defuncts.Find(id);
            if (defunct == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.defuncts)
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
                defunct = db.defuncts.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                defunct = db.defuncts.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                defunct = db.defuncts.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                defunct = db.defuncts.Find(ViewBag.last);
            }

            return View(defunct);
        }

        // GET: defunctbas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: defunctbas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] defunct defunctbas)
        {
            if (ModelState.IsValid)
            {
                db.defuncts.Add(defunctbas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(defunctbas);
        }

        // GET: defunctbas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            defunct defunctbas = db.defuncts.Find(id);
            if (defunctbas == null)
            {
                return HttpNotFound();
            }
            return View(defunctbas);
        }

        // POST: defunctbas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] defunct defunctbas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(defunctbas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(defunctbas);
        }

        // GET: defunctbas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            defunct defunctbas = db.defuncts.Find(id);
            if (defunctbas == null)
            {
                return HttpNotFound();
            }
            return View(defunctbas);
        }

        // POST: defunctbas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            defunct defunctbas = db.defuncts.Find(id);
            db.defuncts.Remove(defunctbas);
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
