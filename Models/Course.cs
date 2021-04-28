using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoursesApp.Models
{
    public class Course
    {
        public int Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Display(Name = "Предмет")]
        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Кредити")]
        public int Credits { get; set; }

        [Required]
        [Display(Name = "Семестар")]
        public int Semester { get; set; }

        [Display(Name = "Студиска програма")]
        [StringLength(100)]
        public string Programme { get; set; }

        [Display(Name = "Степен")]
        [StringLength(25)]
        public string EducationLevel { get; set; }

        [Display(Name = "Професор")]
        public int? FirstTeacherId { get; set; }
        [Display(Name = "Професор")]
        public Teacher FirstTeacher { get; set; }

        [Display(Name = "Втор професор")]
        public int? SecondTeacherId { get; set; }
        [Display(Name = "Втор професор")]
        public Teacher SecondTeacher { get; set; }

        [Display(Name = "Запишани студенти")]
        public ICollection<Enrollment> Students { get; set; }
    }
}
