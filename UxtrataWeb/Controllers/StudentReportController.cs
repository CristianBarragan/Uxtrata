using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UxtrataWeb.Business;
using UxtrataWeb.Models;
using UxtrataWeb.ModelView;
using UxtrataWeb.Util;

namespace UxtrataWeb.Controllers
{
    public class StudentReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ReportBusiness business;

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
            CourseReportViewModel model = new CourseReportViewModel
            {
                Students = studentList,
            };
            return View(model);
        }

        public ActionResult Report(string id, int studentId)
        {
            business = new ReportBusiness();
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Reports"), "StudentReport.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            //ReportDataSource rd = new ReportDataSource("studentReportDataSet", business.getReportStudents(studentId));
            //lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        public JsonResult GetTransactions(int id)
        {
            business = new ReportBusiness();
            return Json(business.getReportStudents(id), JsonRequestBehavior.AllowGet);
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