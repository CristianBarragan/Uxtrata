using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    [MetadataType(typeof(Course.MetaData))]
    public partial class Course
    {
        internal class MetaData
        {
            [RegularExpression(@"^.{5,}$", ErrorMessage = "Minimum 3 characters required")]
            [Required(ErrorMessage = "Required")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
            [DisplayName("Course Name")]
            public string CourseName { get; set; }

            [Required(ErrorMessage = "Required")]
            [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
            [DisplayName("Course Cost")]
            public decimal Cost { get; set; }
        }
    }
}