using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.ModelView
{
    public class CourseDTO
    {
        public int CourseSelectionID { get; set; }
        public string CourseName { get; set; }
        public decimal Balance { get; set; }
        public decimal CourseCost { get; set; }
    }
}