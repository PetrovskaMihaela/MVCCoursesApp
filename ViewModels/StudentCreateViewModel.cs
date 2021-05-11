using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Models;

namespace UniCoursesApp.ViewModels
{
    public class StudentCreateViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Индекс")]
        [StringLength(10)]
        public string StudentId { get; set; }

        [Required]
        [Display(Name = "Име")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Презиме")]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Датум на упис")]
        public DateTime? EnrollmentDate { get; set; }

        [Display(Name = "Освоени кредити")]
        public int? AcquiredCredits { get; set; }

        [Display(Name = "Семестар")]
        public int? CurrentSemester { get; set; }

        [Display(Name = "Степен")]
        [StringLength(25)]
        public string EducationLevel { get; set; }


        [Display(Name = "Слика")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "Име и презиме")]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        // [Display(Name = "Запишани предмети")]
        public ICollection<Enrollment> Courses { get; set; }
    }
}
