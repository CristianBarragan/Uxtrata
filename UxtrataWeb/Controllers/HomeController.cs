using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UxtrataWeb.Models;
using UxtrataWeb.ModelView;
using UxtrataWeb.Util;

namespace UxtrataWeb.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var student = db.Student.ToList();
            var studentList = student.Select(s => new UxtrataWeb.ModelView.SelectListItem()
            {
                Text = s.Name,
                Value = s.StudentID.ToString(),
            }).ToList();
            studentList.Insert(0, empty());
            HomeViewModel model = new HomeViewModel
            {
                Students = studentList,
            };
            return View(model);
        }

        public JsonResult GetCourses(int id)
        {
            List<CourseDTO> coursesView = (from s in db.Student
                              join cs in db.CourseSelection on s.StudentID equals cs.StudentID
                              join c in db.Course on cs.CourseID equals c.CourseID
                                   where s.StudentID == id
                              select new CourseDTO
                              {
                                  CourseSelectionID = cs.ID,
                                  CourseName = c.CourseName,
                                  CourseCost= cs.CourseCost,
                                  Balance = cs.Balance
                              }).ToList();
            return Json(coursesView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MakePayment(int CourseSelectionID, string Value)
        {
            int CsId = CourseSelectionID;
            var val = Convert.ToDecimal(Value);
            if (val <= 0)
            {
                return Json("Payment should be greater than 0", JsonRequestBehavior.AllowGet);
            }
            using (var dbContextTransaction = db.Database.BeginTransaction())
            {
                try
                {
                    CourseSelection cs = db.CourseSelection.Find(CsId);
                    if (cs == null)
                    {
                        dbContextTransaction.Rollback();
                        return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                        if ((cs.Balance*(-1)) + val > 0)
                        {
                            dbContextTransaction.Rollback();
                            return Json("Payment is greater than owed", JsonRequestBehavior.AllowGet);
                        }
                        cs.Balance = cs.Balance - val;
                        if (ModelState.IsValid)
                        {
                            db.Entry(cs).State = EntityState.Modified;
                            var transaction = (from csc in db.CourseSelection
                                               join c in db.Course on csc.CourseID equals c.CourseID
                                               join s in db.Student on csc.StudentID equals s.StudentID
                                               where csc.ID == CsId
                                               select new
                                               {
                                                   DebitAccountID = c.AccountID,
                                                   CreditAccountID = s.AccountID,
                                                   Amount = val
                                               }).FirstOrDefault();
                            if (transaction == null)
                            {
                                dbContextTransaction.Rollback();
                                return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var businessType = db.BusinessType.Where(bt => bt.Name == Constants.BusinessType.Enroll_Student.ToString()).FirstOrDefault();
                                if (businessType == null)
                                {
                                    dbContextTransaction.Rollback();
                                    return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                                }
                                Transaction t = new Transaction();
                                t.BusinessID = CsId;
                                t.DebitAccountID = transaction.DebitAccountID;
                                t.CreditAccountID = transaction.CreditAccountID;
                                t.Amount = transaction.Amount;
                                t.BusinessTypeID = businessType.BusinessTypeID;
                                db.Transaction.Add(t);
                                var CreditAccount = db.Account.Where(a => a.AccountID == transaction.CreditAccountID).FirstOrDefault();
                                var DebitAccount = db.Account.Where(a => a.AccountID == transaction.DebitAccountID).FirstOrDefault();
                                if ((CreditAccount == null) || (DebitAccount == null))
                                {
                                    dbContextTransaction.Rollback();
                                    return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                                }
                                if(CreditAccount.Balance < transaction.Amount)
                                {
                                    dbContextTransaction.Rollback();
                                    return Json("Credit Account does not have enough funds", JsonRequestBehavior.AllowGet);
                                }
                                CreditAccount.Balance = CreditAccount.Balance - transaction.Amount;
                                DebitAccount.Balance = DebitAccount.Balance + transaction.Amount;
                                db.Entry(CreditAccount).State = EntityState.Modified;
                                db.Entry(DebitAccount).State = EntityState.Modified;
                                db.SaveChanges();
                                dbContextTransaction.Commit();
                                return Json("Transaction successful", JsonRequestBehavior.AllowGet);
                            }                     
                        }
                        else
                        {
                            dbContextTransaction.Rollback();
                            return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    return Json("Transaction failed", JsonRequestBehavior.AllowGet);
                }
            }
        }

        private UxtrataWeb.ModelView.SelectListItem empty()
        {
            UxtrataWeb.ModelView.SelectListItem emptyItem = new UxtrataWeb.ModelView.SelectListItem()
            {
                Text = "Select",
                Value = "0",
            };
            return emptyItem;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
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