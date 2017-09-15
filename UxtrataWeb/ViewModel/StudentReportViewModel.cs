using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.ModelView
{
    public partial class CourseReportViewModel
    {
        public IEnumerable<SelectListItem> Students { get; set; }
        public string Student { get; set; }
    }
}