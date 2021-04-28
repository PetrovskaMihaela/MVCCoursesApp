using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Models;

namespace UniCoursesApp.ViewModels
{
    public class StudentNameIndexViewModel
    {
        public IList<Student> StudentVm { get; set; }
        public SelectList IndexVm { get; set; }
        public string StudentIndex { get; set; }
        public string SearchString { get; set; }
    }
}
