using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    public partial class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int AccountID { get; set; }
        public decimal Cost { get; set; }

        public virtual ICollection<CourseSelection> CourseSelections { get; set; }

        public virtual Account Account { get; set; }
    }
}