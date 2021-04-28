using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoursesApp.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }


        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }


        [Display(Name = "Семестар")]
        [StringLength(10)]
        public string Semester { get; set; }

        [Display(Name = "Година")]
        public int? Year { get; set; }

        [Display(Name = "Оценка")]
        public int? Grade { get; set; }

        [Display(Name = "Погледни колоквиум")]
        [StringLength(255)]
        public string SeminalUrl { get; set; }

        [Display(Name = "Погледни проект")]
        [StringLength(255)]
        public string ProjectUrl { get; set; }

        [Display(Name = "Поени од испит")]
        public int? ExamPoints { get; set; }

        [Display(Name = "Поени од колоквиум")]
        public int? SeminalPoints { get; set; }

        [Display(Name = "Поени од проект")]
        public int? ProjectPoints { get; set; }

        [Display(Name = "Дополнителни поени")]
        public int? AdditionalPoints { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Крај")]
        public DateTime? FinishDate { get; set; }
    }
}
