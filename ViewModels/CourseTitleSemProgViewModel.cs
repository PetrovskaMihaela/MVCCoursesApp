using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Models;

namespace UniCoursesApp.ViewModels
{
    public class CourseTitleSemProgViewModel
    {
        public IList<Course> CoursesVm { get; set; }
        public SelectList SemesterVm { get; set; }
        public SelectList ProgrammeVm { get; set; }
        public int CourseSemester { get; set; }
        public string CourseProgramme{ get; set; }
        public string SearchString { get; set; }


    }
}
