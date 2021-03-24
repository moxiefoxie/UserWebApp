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
    public class JournalEntriesController : Controller
    {
        private UserWebApp_dbEntities1 db = new UserWebApp_dbEntities1();

        // GET: JournalEntries
        public ActionResult Index()
        {
            return View(db.JournalEntries.ToList());
        }

        // GET: JournalEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JournalEntry journalEntry = db.JournalEntries.Find(id);
            if (journalEntry == null)
            {
                return HttpNotFound();
            }
            return View(journalEntry);
        }

        // GET: JournalEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JournalEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JournalID,DebitAccountNumber,CreditAccountNumber,Amount,Status")] JournalEntry journalEntry, ChartOfAccount chartOfAccount)
        {
            if (ModelState.IsValid)
            {
                bool journalIdExists = false;
                bool debitAccountExists = false;
                bool creditAccountExists = false;
                foreach (JournalEntry je in db.JournalEntries)
                {
                    if (je.JournalID != journalEntry.JournalID)
                    {
                        continue;
                    }
                    else
                    {
                        journalIdExists = true;
                        break;
                    }
                }

                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber != journalEntry.DebitAccountNumber)
                    {
                        continue;
                    }
                    else if(c.AccountNumber == journalEntry.DebitAccountNumber && c.NormalSide.Trim() == "Debit")
                    {
                        debitAccountExists = true;
                        break;
                    }
                    
                }

                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber != journalEntry.CreditAccountNumber)
                    {
                        continue;
                    }
                    else if(c.AccountNumber == journalEntry.CreditAccountNumber && c.NormalSide.Trim() == "Credit")
                    {
                        creditAccountExists = true;
                        break;
                    }
                }

                //True True True
                if (journalIdExists && debitAccountExists && creditAccountExists)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                }

                //True True False
                if (journalIdExists && debitAccountExists && !creditAccountExists)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }
                //True False True
                if (journalIdExists && !debitAccountExists & creditAccountExists)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                }
                //True False False
                if (journalIdExists && !debitAccountExists && !creditAccountExists)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }
                //False True True
                if (!journalIdExists && debitAccountExists && creditAccountExists)
                {
                    journalEntry.Status = "Pending";
                    db.JournalEntries.Add(journalEntry);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //False True False
                if (!journalIdExists && debitAccountExists && !creditAccountExists)
                {
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }
                //False False True
                if (!journalIdExists && !debitAccountExists && creditAccountExists)
                {
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                }
                //False False False
                if (!journalIdExists && !debitAccountExists && !creditAccountExists)
                {
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }


                //journalEntry.Status = "Pending Approval";
                //db.JournalEntries.Add(journalEntry);
                //db.SaveChanges();
                
            }

            return View(journalEntry);
        }

        // GET: JournalEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JournalEntry journalEntry = db.JournalEntries.Find(id);
            if (journalEntry == null)
            {
                return HttpNotFound();
            }
            return View(journalEntry);
        }

        // POST: JournalEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JournalID,DebitAccountNumber,CreditAccountNumber,Amount,Status")] JournalEntry journalEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(journalEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(journalEntry);
        }

        // GET: JournalEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JournalEntry journalEntry = db.JournalEntries.Find(id);
            if (journalEntry == null)
            {
                return HttpNotFound();
            }
            return View(journalEntry);
        }

        // POST: JournalEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JournalEntry journalEntry = db.JournalEntries.Find(id);
            db.JournalEntries.Remove(journalEntry);
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
