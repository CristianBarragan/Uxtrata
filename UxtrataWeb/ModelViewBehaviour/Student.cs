using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    [MetadataType(typeof(Student.MetaData))]
    public partial class Student
    {
        internal class MetaData
        {
            [RegularExpression(@"^.{5,}$", ErrorMessage = "Minimum 3 characters required")]
            [Required(ErrorMessage = "Required")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "Invalid")]
            [DisplayName("Name")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Required")]
            [Range(0, Int32.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
            [DisplayName("Age")]
            public int Age { get; set; }
        }
    }
}