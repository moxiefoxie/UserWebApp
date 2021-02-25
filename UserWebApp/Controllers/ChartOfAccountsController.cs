using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserWebApp.Models;

namespace UserWebApp.Controllers
{
    public class ChartOfAccountsController : Controller
    {
        private Entities2 db = new Entities2();

        // GET: ChartOfAccounts
        public ActionResult Index()
        {
            return View(db.ChartOfAccounts.ToList());
        }

        // GET: ChartOfAccounts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            if (chartOfAccount == null)
            {
                return HttpNotFound();
            }
            return View(chartOfAccount);
        }

        // GET: ChartOfAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChartOfAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountNumber,AccoutnDesc,NormalSide,Category,Subcategory,InitialBalance,Debit,Credit,DateCreated,Balance,UserID,Order,Statement,Comment,AccountName")] ChartOfAccount chartOfAccount)
        {
            if (ModelState.IsValid)
            {
                db.ChartOfAccounts.Add(chartOfAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chartOfAccount);
        }

        // GET: ChartOfAccounts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            if (chartOfAccount == null)
            {
                return HttpNotFound();
            }
            return View(chartOfAccount);
        }

        // POST: ChartOfAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountNumber,AccoutnDesc,NormalSide,Category,Subcategory,InitialBalance,Debit,Credit,DateCreated,Balance,UserID,Order,Statement,Comment,AccountName")] ChartOfAccount chartOfAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chartOfAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chartOfAccount);
        }

        // GET: ChartOfAccounts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            if (chartOfAccount == null)
            {
                return HttpNotFound();
            }
            return View(chartOfAccount);
        }

        // POST: ChartOfAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);
            db.ChartOfAccounts.Remove(chartOfAccount);
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
