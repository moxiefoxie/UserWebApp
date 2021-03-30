using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserWebApp.Models;
using System.Data.Entity.Validation;

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
        public ActionResult Create([Bind(Include = "JournalID,DebitAccountNumber,CreditAccountNumber,Amount,Status,DateApproved,Comment")] JournalEntry journalEntry, ChartOfAccount chartOfAccount)
        {
            if (ModelState.IsValid)
            {
                bool journalIdExists = false;
                bool debitAccountExists = false;
                bool creditAccountExists = false;
                bool validBalance = false;
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
                    else if (c.AccountNumber == journalEntry.DebitAccountNumber && c.NormalSide.Trim() == "Debit")
                    {

                        debitAccountExists = true;
                        if (c.Balance >= journalEntry.Amount)
                        {
                            validBalance = true;
                        }

                        break;


                    }

                }

                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber != journalEntry.CreditAccountNumber)
                    {
                        continue;
                    }
                    else if (c.AccountNumber == journalEntry.CreditAccountNumber && c.NormalSide.Trim() == "Credit")
                    {
                        creditAccountExists = true;
                        break;
                    }
                }

                //True True True True
                if (journalIdExists && debitAccountExists && creditAccountExists && validBalance)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                }
                //True True True False
                if (journalIdExists && debitAccountExists && creditAccountExists && !validBalance)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("DebitAccountNumber", "There is not enough money in this account to debit the given amount");
                }
                //True True False True
                if (journalIdExists && debitAccountExists && !creditAccountExists && validBalance)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }

                //True True False False
                if (journalIdExists && debitAccountExists && !creditAccountExists && !validBalance)
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                    ModelState.AddModelError("DebitAccountNumber", "There is not enough money in this account to debit the given amount");
                }
                //True False True True
                if (journalIdExists && !debitAccountExists && creditAccountExists)//if the account doesnt exist then validbalance will always be false
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                }

                //True False False True
                if (journalIdExists && !debitAccountExists && !creditAccountExists)//if account doesnt exist then validBalance will always be false
                {
                    ModelState.AddModelError("JournalID", "An journal Entry with this ID already exists");
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                }
                //False True True
                if (!journalIdExists && debitAccountExists && creditAccountExists && validBalance)
                {
                    journalEntry.Status = "Pending";
                    journalEntry.DateApproved = DateTime.Today;
                    journalEntry.Comment = "No Comment";
                    db.JournalEntries.Add(journalEntry);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                //False True False
                if (!journalIdExists && debitAccountExists && !creditAccountExists && !validBalance)
                {
                    ModelState.AddModelError("CreditAccountNumber", "A credit account with this ID does not exist");
                    ModelState.AddModelError("DebitAccountNumber", "There is not enough money in this account to debit the given amount");
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
                    ModelState.AddModelError("DebitAccountNumber", "A debit account with this ID does not exist");
                }
                if (!journalIdExists && debitAccountExists && creditAccountExists && !validBalance)
                {
                    ModelState.AddModelError("DebitAccountNumber", "There is not enough money in this account to debit the given amount");
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
                //db.Entry(journalEntry).State = EntityState.Modified;
                //db.SaveChanges();
                //change status to approved
                journalEntry.Status = "Approved";
                db.Entry(journalEntry).State = EntityState.Modified;


                Ledger ledDebit = new Ledger();
                ledDebit.LedgerID = journalEntry.JournalID;
                ledDebit.DebitAccountNumber = journalEntry.DebitAccountNumber;
                ledDebit.CreditAccountNumber = journalEntry.CreditAccountNumber;
                ledDebit.Status = "Approved";
                ledDebit.Amount = journalEntry.Amount;
                ledDebit.DateApproved = DateTime.Today;
                ledDebit.DateChanged = DateTime.Today;
                ledDebit.Comment = "Ledger approved";

                Ledger ledCredit = new Ledger();
                ledCredit.LedgerID = journalEntry.JournalID + 1;
                ledCredit.DebitAccountNumber = journalEntry.DebitAccountNumber;
                ledCredit.CreditAccountNumber = journalEntry.CreditAccountNumber;
                ledCredit.Amount = journalEntry.Amount;
                ledCredit.Status = "Approved";
                ledCredit.DateApproved = DateTime.Today;
                ledCredit.DateChanged = DateTime.Today;
                ledCredit.Comment = "Ledger approved";


                //update chart of accounts
                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber == journalEntry.DebitAccountNumber && c.NormalSide.Trim() == "Debit")
                    {
                        c.Balance = c.Balance - journalEntry.Amount;
                        ledDebit.NewBalance = c.Balance;
                        break;
                    }
                }

                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber == journalEntry.CreditAccountNumber && c.NormalSide.Trim() == "Credit")
                    {
                        c.Balance = c.Balance + journalEntry.Amount;
                        ledCredit.NewBalance = c.Balance;
                        break;
                    }
                }

                

                
                    
                    //db.SaveChanges();

                try
                {
                    db.Ledgers.Add(ledDebit);
                    db.Ledgers.Add(ledCredit);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                    {
                        // Get entry

                        System.Data.Entity.Infrastructure.DbEntityEntry entry = item.Entry;
                        string entityTypeName = entry.Entity.GetType().Name;

                        // Display or log error messages

                        foreach (DbValidationError subItem in item.ValidationErrors)
                        {
                            string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                     subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                            Console.WriteLine(message);
                        }
                    }
                }

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


        public ActionResult Approve(JournalEntry journalEntry)
        {

            //change status to approved
            journalEntry.Status = "Approved";


            Ledger ledDebit = new Ledger();
            ledDebit.DebitAccountNumber = journalEntry.DebitAccountNumber;
            ledDebit.CreditAccountNumber = journalEntry.CreditAccountNumber;
            ledDebit.Status = "Approved";
            ledDebit.Amount = journalEntry.Amount;
            ledDebit.DateApproved = DateTime.Today;
            ledDebit.DateChanged = DateTime.Today;
            ledDebit.Comment = "Ledger approved";

            Ledger ledCredit = new Ledger();
            ledCredit.DebitAccountNumber = journalEntry.DebitAccountNumber;
            ledCredit.CreditAccountNumber = journalEntry.CreditAccountNumber;
            ledCredit.Amount = journalEntry.Amount;
            ledCredit.Status = "Approved";
            ledCredit.DateApproved = DateTime.Today;
            ledCredit.DateChanged = DateTime.Today;
            ledCredit.Comment = "Ledger approved";


            //update chart of accounts
            foreach (ChartOfAccount c in db.ChartOfAccounts)
            {
                if (c.AccountNumber == journalEntry.DebitAccountNumber && c.NormalSide.Trim() == "Debit")
                {
                    c.Balance = c.Balance - journalEntry.Amount;
                    ledDebit.NewBalance = c.Balance;
                    break;
                }
            }

            foreach (ChartOfAccount c in db.ChartOfAccounts)
            {
                if (c.AccountNumber == journalEntry.CreditAccountNumber && c.NormalSide.Trim() == "Credit")
                {
                    c.Balance = c.Balance + journalEntry.Amount;
                    ledCredit.NewBalance = c.Balance;
                    break;
                }
            }

            db.Ledgers.Add(ledDebit);
            db.Ledgers.Add(ledCredit);

            return View("~/Views/JournalEntries/Index.cshtml");
        }



    }
}