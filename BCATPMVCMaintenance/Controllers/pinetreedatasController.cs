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
    public class PinetreedatasController : Controller
    {
        private readonly BCATPEntities db = new BCATPEntities();
        private readonly List<int> allTheSheets = new List<int>();
        // GET: pinetreedatas
        public ActionResult Index(string searchString)
        {
            var pinetree = from s in db.pinetrees
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                pinetree = pinetree.Where(s => s.name.Contains(searchString));
            }
            return View(pinetree.ToList());
        }

        // GET: pinetreedatas/Details/5
        public ActionResult Details(int? id, string value)
        {
            pinetree pinetree = db.pinetrees.Find(id);
            if (pinetree == null)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong, invalid model");
            }

            foreach (var item in db.pinetrees)
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
                pinetree = db.pinetrees.Find(ViewBag.allKeys[index + 1]);
            }
            if (index + 1 > ViewBag.allKeys.Count - 1 && value == "next")
            {
                pinetree = db.pinetrees.Find(ViewBag.first);
            }
            if (index - 1 >= -1 && value == "prev" && index != 0)
            {
                pinetree = db.pinetrees.Find(ViewBag.allKeys[index - 1]);
            }
            if (index == 0 && value == "prev")
            {
                pinetree = db.pinetrees.Find(ViewBag.last);
            }

            return View(pinetree);
        }

        // GET: pinetreedatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: pinetreedatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] pinetree pinetreedata)
        {
            if (ModelState.IsValid)
            {
                db.pinetrees.Add(pinetreedata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pinetreedata);
        }

        // GET: pinetreedatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pinetree pinetreedata = db.pinetrees.Find(id);
            if (pinetreedata == null)
            {
                return HttpNotFound();
            }
            return View(pinetreedata);
        }

        // POST: pinetreedatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Latitude,Longitude,Comment,Wiki")] pinetree pinetreedata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pinetreedata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pinetreedata);
        }

        // GET: pinetreedatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pinetree pinetreedata = db.pinetrees.Find(id);
            if (pinetreedata == null)
            {
                return HttpNotFound();
            }
            return View(pinetreedata);
        }

        // POST: pinetreedatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pinetree pinetreedata = db.pinetrees.Find(id);
            db.pinetrees.Remove(pinetreedata);
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
