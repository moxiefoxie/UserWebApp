using Audit.Mvc;
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
        private UserWebApp_dbEntities db = new UserWebApp_dbEntities();
        

        public ActionResult EventLog(int id)
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

        // GET: ChartOfAccounts
        public ActionResult Index(String sortOrder, String searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NumberSortParm = sortOrder == "ascending" ? "num_desc" : "ascending";
            var accounts = from a in db.ChartOfAccounts select a;


            if (!String.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(s => s.AccountName.Contains(searchString));
            }
            
            switch(sortOrder)
            {
                case "name_desc":
                    accounts = accounts.OrderByDescending(a => a.AccountName);
                    break;
                case "ascending":
                    accounts = accounts.OrderBy(a => a.AccountNumber);
                    break;
                case "num-desc":
                    accounts = accounts.OrderByDescending(a => a.AccountNumber.ToString());
                    break;
                default:
                    accounts = accounts.OrderBy(a => a.AccountName);
                    break;
            }

            return View(accounts);
        }

        // GET: ChartOfAccounts/Details/5
        public ActionResult Details(int id)
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
        public ActionResult Create([Bind(Include = "AccountNumber,AccountDescription,NormalSide,Category,Subcategory,InitialBalance,Debit,Credit,DateCreated,Balance,UserID,Orders,Statements,Comment,AccountName")] ChartOfAccount chartOfAccount)
        {
            Boolean nameExists = false;
            Boolean numExists = false;
            if (ModelState.IsValid)
            {
                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountName != chartOfAccount.AccountName)
                    {
                        continue;
                    }
                    else
                    {
                        nameExists = true;
                        break;
                    }
                }
                
                foreach (ChartOfAccount c in db.ChartOfAccounts)
                {
                    if (c.AccountNumber != chartOfAccount.AccountNumber)
                    {
                        continue;
                    }
                    else
                    {
                        numExists = true;
                        break;
                    }
                }

                if (!nameExists && !numExists)
                {
                    db.ChartOfAccounts.Add(chartOfAccount);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                if(nameExists && !numExists)
                {
                    ModelState.AddModelError("AccountName","An account with this name already exists");
                }
                if(!nameExists && numExists)
                {
                    ModelState.AddModelError("AccountNumber", "An account with this number already exists");
                }
                if(nameExists && numExists)
                {
                    ModelState.AddModelError("AccountName", "An account with this name already exists");
                    ModelState.AddModelError("AccountNumber", "An account with this number already exists");
                }

                
            }

            return View(chartOfAccount);
        }

        
        // GET: ChartOfAccounts/Edit/5
        
        public ActionResult Edit(int id)
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
        public ActionResult Edit([Bind(Include = "AccountNumber,AccountDescription,NormalSide,Category,Subcategory,InitialBalance,Debit,Credit,DateCreated,Balance,UserID,Orders,Statements,Comment,AccountName")] ChartOfAccount chartOfAccount)
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
        public ActionResult Delete(int id)
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
        public ActionResult DeleteConfirmed(int id)
        {
            ChartOfAccount chartOfAccount = db.ChartOfAccounts.Find(id);

            if(chartOfAccount.Balance > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                db.ChartOfAccounts.Remove(chartOfAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Ledger(int id)
        {
            if(id == null)
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

        public ActionResult JournalEntry()
        {
            
            return View();
        }

        
    }
}
