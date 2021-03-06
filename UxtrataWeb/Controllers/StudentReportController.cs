﻿using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            StudentReportViewModel model = new StudentReportViewModel
            {
                Students = studentList,
            };
            return View(model);
        }

        [HttpGet]
        public FileContentResult Report()
        {
            return (FileContentResult)Session["FileResult"];
        }

        [HttpPost]
        public HttpStatusCodeResult Report(string formatId, int studentId)
        {
            return generateReport(formatId, studentId);
        }

        private HttpStatusCodeResult generateReport(string formatId, int studentId)
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportDataSource rd = new ReportDataSource("studentReportDataSet", business.getReportStudents(studentId));
            lr.DataSources.Add(rd);
            string reportType = formatId;
            string mimeType;
            string encoding;
            string fileNameExtension;

            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + formatId + "</OutputFormat>" +
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
            Session["FileResult"] = File(renderedBytes, mimeType);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
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