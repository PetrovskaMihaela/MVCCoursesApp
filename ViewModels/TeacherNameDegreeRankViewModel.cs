using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Models;

namespace UniCoursesApp.ViewModels
{
    public class TeacherNameDegreeRankViewModel
    {
        public IList<Teacher> TeachersVm { get; set; }
        public SelectList DegreeVm { get; set; }
        public SelectList RankVm { get; set; }
        public string TeacherDegree { get; set; }
        public string TeacherRank { get; set; }
        public string SearchString { get; set; }
    }
}
