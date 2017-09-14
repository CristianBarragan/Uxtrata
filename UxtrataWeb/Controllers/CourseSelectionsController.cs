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
    public class CourseSelectionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseSelections
        public ActionResult Index()
        {
            var courseSelection = db.CourseSelection.Include(c => c.Course).Include(c => c.Student);
            return View(courseSelection.ToList());
        }

        // GET: CourseSelections/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseSelection courseSelection = db.CourseSelection.Find(id);
            if (courseSelection == null)
            {
                return HttpNotFound();
            }
            return View(courseSelection);
        }

        // GET: CourseSelections/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName");
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "Name");
            return View();
        }

        // POST: CourseSelections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StudentID,CourseID,CourseCost,Balance")] CourseSelection courseSelection)
        {
            
            decimal amount = courseSelection.Balance;
            int courseId = courseSelection.CourseID;
            var course = db.Course.Where(c => c.CourseID == courseId).FirstOrDefault();
            
            if(course == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    courseSelection.CourseCost = course.Cost;
                    if (courseSelection.Balance > 0)
                    {
                        courseSelection.Balance = (courseSelection.CourseCost - courseSelection.Balance);
                    }
                    else
                    {
                        courseSelection.Balance = courseSelection.CourseCost;
                    }

                    if (ModelState.IsValid)
                    {
                        db.CourseSelection.Add(courseSelection);
                        db.SaveChanges();
                        if(amount > 0)
                        {
                            int studentId = courseSelection.StudentID;
                            var student = db.Student.Where(c => c.StudentID == studentId).FirstOrDefault();
                            var businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Enroll_Student.ToString()).FirstOrDefault();
                            if (student == null)
                            {
                                dbContextTransaction.Rollback();
                                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                            }
                            Transaction t = new Transaction();
                            t.BusinessID = courseSelection.ID;
                            t.DebitAccountID = course.AccountID;
                            t.CreditAccountID = student.AccountID;
                            t.Amount = amount;
                            t.BusinessTypeID = businessType.BusinessTypeID;
                            db.Transaction.Add(t);

                            var CreditAccount = db.Account.Where(a => a.AccountID == t.CreditAccountID).FirstOrDefault();
                            var DebitAccount = db.Account.Where(a => a.AccountID == t.DebitAccountID).FirstOrDefault();
                            if ((CreditAccount == null) || (DebitAccount == null))
                            {
                                dbContextTransaction.Rollback();
                                return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                            }
                            if (CreditAccount.Balance < t.Amount)
                            {
                                dbContextTransaction.Rollback();
                                return Json("Credit Account does not have enough funds", JsonRequestBehavior.AllowGet);
                            }
                            CreditAccount.Balance = CreditAccount.Balance - t.Amount;
                            DebitAccount.Balance = DebitAccount.Balance + t.Amount;
                            db.Entry(CreditAccount).State = EntityState.Modified;
                            db.Entry(DebitAccount).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        dbContextTransaction.Commit();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
        }

        // GET: CourseSelections/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseSelection courseSelection = db.CourseSelection.Find(id);
            if (courseSelection == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", courseSelection.CourseID);
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "Name", courseSelection.StudentID);
            return View(courseSelection);
        }

        // POST: CourseSelections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StudentID,CourseID,CourseCost,Balance")] CourseSelection courseSelection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseSelection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Course, "CourseID", "CourseName", courseSelection.CourseID);
            ViewBag.StudentID = new SelectList(db.Student, "StudentID", "Name", courseSelection.StudentID);
            return View(courseSelection);
        }

        // GET: CourseSelections/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseSelection courseSelection = db.CourseSelection.Find(id);
            if (courseSelection == null)
            {
                return HttpNotFound();
            }
            return View(courseSelection);
        }

        // POST: CourseSelections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    CourseSelection courseSelection = db.CourseSelection.Find(id);
                    var transaction = (from cs in db.CourseSelection
                                join s in db.Student on cs.StudentID equals s.StudentID
                                join c in db.Course on cs.CourseID equals c.CourseID
                                where cs.ID == id
                                select new
                                {
                                    CourseSelectionID = cs.ID,
                                    CreditAccountID = c.AccountID,
                                    DebitAccountID = s.AccountID,
                                    Balance = cs.Balance,
                                    CourseCost = cs.CourseCost
                                }).FirstOrDefault();

                    if (transaction == null)
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Enroll_Student.ToString()).FirstOrDefault();
                    if (businessType == null)
                    {
                        dbContextTransaction.Rollback();
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    Transaction t = new Transaction();
                    t.DebitAccountID = transaction.DebitAccountID;
                    t.CreditAccountID = transaction.CreditAccountID;
                    t.Amount = transaction.CourseCost - transaction.Balance;
                    t.BusinessTypeID = businessType.BusinessTypeID;
                    db.Transaction.Add(t);
                    var CreditAccount = db.Account.Where(a => a.AccountID == transaction.CreditAccountID).FirstOrDefault();
                    var DebitAccount = db.Account.Where(a => a.AccountID == transaction.DebitAccountID).FirstOrDefault();
                    if ((CreditAccount == null) || (DebitAccount == null))
                    {
                        dbContextTransaction.Rollback();
                        return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                    }
                    if (CreditAccount.Balance < (transaction.CourseCost - transaction.Balance))
                    {
                        dbContextTransaction.Rollback();
                        return Json("Credit Account does not have enough funds", JsonRequestBehavior.AllowGet);
                    }
                    CreditAccount.Balance = CreditAccount.Balance - (transaction.CourseCost - transaction.Balance);
                    DebitAccount.Balance = DebitAccount.Balance + (transaction.CourseCost - transaction.Balance);
                    db.Entry(CreditAccount).State = EntityState.Modified;
                    db.Entry(DebitAccount).State = EntityState.Modified;
                    db.CourseSelection.Remove(courseSelection);
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
