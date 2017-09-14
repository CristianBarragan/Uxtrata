using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UxtrataWeb.Models;
using UxtrataWeb.ModelView;
using UxtrataWeb.Util;

namespace UxtrataWeb.Controllers
{
    public class StudentReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentReport
        public ActionResult Index()
        {
            var student = db.Student.ToList();
            var studentList = student.Select(s => new UxtrataWeb.ModelView.SelectListItem()
            {
                Text = s.Name,
                Value = s.StudentID.ToString(),
            }).ToList();
            studentList.Insert(0, empty());
            StudentReportViewModel model = new StudentReportViewModel
            {
                Students = studentList,
            };
            return View(model);
        }

        public JsonResult GetTransactions(int id)
        {
            //Add enroll student transactions
            List<TransactionDTO> transactions = (from s in db.Student
                                                 join cs in db.CourseSelection on s.StudentID equals cs.StudentID
                                                 join c in db.Course on cs.CourseID equals c.CourseID
                                                 join t in db.Transaction on cs.ID equals t.BusinessID
                                                 join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                                 join da in db.Account on t.DebitAccountID equals da.AccountID
                                                 join ca in db.Account on t.CreditAccountID equals ca.AccountID
                                                where s.StudentID == id && bt.Name == Constants.BusinessType.Enroll_Student.ToString()
                                                select new TransactionDTO
                                                 {
                                                     TransactionID = t.TransactionID,
                                                     TransactionType = bt.Name,
                                                     CourseName = c.CourseName,
                                                     DebitAccountCode = da.Code,
                                                     CreditAccountCode = ca.Code,
                                                     Amount = t.Amount
                                                 }).ToList();
            //Add deposit transactions
            transactions.AddRange((from s in db.Student
                                   join a in db.Account on s.AccountID equals a.AccountID
                                   join t in db.Transaction on a.AccountID equals t.DebitAccountID
                                   join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                   where s.StudentID == id && bt.Name == Constants.BusinessType.Deposit.ToString()
                                   select new TransactionDTO
                                   {
                                       TransactionID = t.TransactionID,
                                       TransactionType = bt.Name,
                                       CourseName = "N/A",
                                       DebitAccountCode = a.Code,
                                       CreditAccountCode = "N/A",
                                       Amount = t.Amount
                                   }).ToList());
            //Add withdraw transactions
            transactions.AddRange((from s in db.Student
                                   join a in db.Account on s.AccountID equals a.AccountID
                                   join t in db.Transaction on a.AccountID equals t.CreditAccountID
                                   join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                   where s.StudentID == id && bt.Name == Constants.BusinessType.Withdraw.ToString()
                                   select new TransactionDTO
                                   {
                                       TransactionID = t.TransactionID,
                                       TransactionType = bt.Name,
                                       CourseName = "N/A",
                                       DebitAccountCode = "N/A",
                                       CreditAccountCode = a.Code,
                                       Amount = t.Amount
                                   }).ToList());
            transactions = transactions.OrderBy(t => t.TransactionID).ToList();
            return Json(transactions, JsonRequestBehavior.AllowGet);
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