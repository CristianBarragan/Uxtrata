using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UxtrataWeb.Models
{
    [MetadataType(typeof(CourseSelection.MetaData))]
    public partial class CourseSelection
    {
        internal class MetaData
        {
            [Required(ErrorMessage = "Required")]
            [Range(0.0, Double.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
            [DisplayName("Owe")]
            public decimal Balance { get; set; }
        }
    }
}