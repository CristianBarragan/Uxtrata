using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UxtrataWeb.Business;
using UxtrataWeb.Models;
using UxtrataWeb.ModelView;

namespace UxtrataWeb.Controllers
{
    public class CourseReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ReportBusiness business;

        // GET: CourseReport
        public ActionResult Index()
        {
            var course = db.Course.ToList();
            var courseList = course.Select(c => new UxtrataWeb.ModelView.SelectListItem()
            {
                Text = c.CourseName,
                Value = c.CourseID.ToString(),
            }).ToList();
            courseList.Insert(0, empty());
            CourseReportViewModel model = new CourseReportViewModel
            {
                Courses = courseList,
            };
            return View(model);
        }

        public JsonResult GetTransactions(int id)
        {
            business = new ReportBusiness();
            return Json(business.getReportCourses(id), JsonRequestBehavior.AllowGet);
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