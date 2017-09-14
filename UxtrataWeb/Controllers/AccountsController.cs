using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UxtrataWeb.Models;
using UxtrataWeb.Util;

namespace UxtrataWeb.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Account.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccountID,Code,Balance")] Account account)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Account.Add(account);
                        db.SaveChanges();
                        var businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Deposit.ToString()).FirstOrDefault();
                        if (businessType == null)
                        {
                            dbContextTransaction.Rollback();
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        Transaction t = new Transaction();
                        t.DebitAccountID = account.AccountID;
                        t.CreditAccountID = 0;
                        t.Amount = account.Balance;
                        t.BusinessTypeID = businessType.BusinessTypeID;
                        db.Transaction.Add(t);
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
            }
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AccountID,Code,Balance")] Account account)
        {
            if (ModelState.IsValid)
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pAccount = db.Account.Find(account.AccountID);
                        BusinessType businessType;
                        Transaction t = new Transaction();
                        if (account.Balance > pAccount.Balance)
                        {
                            businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Deposit.ToString()).FirstOrDefault();
                            t.DebitAccountID = account.AccountID;
                            t.CreditAccountID = 0;
                            t.Amount = account.Balance - pAccount.Balance;
                        }
                        else
                        {
                            businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Withdraw.ToString()).FirstOrDefault();
                            t.DebitAccountID = 0;
                            t.CreditAccountID = account.AccountID;
                            t.Amount = pAccount.Balance - account.Balance;
                        }
                        pAccount.Balance = account.Balance;
                        pAccount.Code = account.Code;
                        if (businessType == null)
                        {
                            dbContextTransaction.Rollback();
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }
                        t.BusinessTypeID = businessType.BusinessTypeID;
                        db.Transaction.Add(t);
                        db.Entry(pAccount).State = EntityState.Modified;
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Account.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    BusinessType businessType;
                    Transaction t = new Transaction();
                    Account account = db.Account.Find(id);
                    db.Student.RemoveRange(db.Student.Where(s => s.AccountID == id));
                    db.Course.RemoveRange(db.Course.Where(c => c.AccountID == id));
                    businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Withdraw.ToString()).FirstOrDefault();
                    t.DebitAccountID = 0;
                    t.CreditAccountID = account.AccountID;
                    t.Amount = account.Balance;
                    if (businessType == null)
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    t.BusinessTypeID = businessType.BusinessTypeID;
                    db.Transaction.Add(t);
                    db.Account.Remove(account);
                    db.SaveChanges();
                    dbContextTransaction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
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
    }
}
