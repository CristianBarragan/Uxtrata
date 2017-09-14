using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    public partial class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int AccountID { get; set; }

        public virtual ICollection<CourseSelection> CourseSelections { get; set; }

        public virtual Account Account { get; set; }
    }
}