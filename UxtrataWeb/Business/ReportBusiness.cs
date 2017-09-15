using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UxtrataWeb.Models;
using UxtrataWeb.ModelView;
using UxtrataWeb.Util;

namespace UxtrataWeb.Business
{
    public class ReportBusiness
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public List<TransactionDTO> getReportStudents(int id)
        {
            using(db = new ApplicationDbContext())
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
                return transactions;
            }
        }

        public List<TransactionDTO> getReportCourses(int id)
        {
            using (db = new ApplicationDbContext())
            {
                //Add enroll student transactions
                List<TransactionDTO> transactions = (from s in db.Student
                                                     join cs in db.CourseSelection on s.StudentID equals cs.StudentID
                                                     join c in db.Course on cs.CourseID equals c.CourseID
                                                     join t in db.Transaction on cs.ID equals t.BusinessID
                                                     join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                                     join da in db.Account on t.DebitAccountID equals da.AccountID
                                                     join ca in db.Account on t.CreditAccountID equals ca.AccountID
                                                     where c.CourseID == id && bt.Name == Constants.BusinessType.Enroll_Student.ToString()
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
                transactions.AddRange((from c in db.Course
                                       join a in db.Account on c.AccountID equals a.AccountID
                                       join t in db.Transaction on a.AccountID equals t.DebitAccountID
                                       join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                       where c.CourseID == id && bt.Name == Constants.BusinessType.Deposit.ToString()
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
                transactions.AddRange((from c in db.Course
                                       join a in db.Account on c.AccountID equals a.AccountID
                                       join t in db.Transaction on a.AccountID equals t.CreditAccountID
                                       join bt in db.BusinessType on t.BusinessTypeID equals bt.BusinessTypeID
                                       where c.CourseID == id && bt.Name == Constants.BusinessType.Withdraw.ToString()
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
                return transactions;
            }
        }
    }
}