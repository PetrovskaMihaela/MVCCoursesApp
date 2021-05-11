using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Models;

namespace UniCoursesApp.ViewModels
{
    public class TeacherCreateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Презиме")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Степен на образование")]
        public string? Degree { get; set; }

        [StringLength(25)]
        [Display(Name = "Звање")]
        public string? AcademicRank { get; set; }

        [StringLength(10)]
        [Display(Name = "Канцеарија")]
        public string? OfficeNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Датум на вработување")]
        public DateTime? HireDate { get; set; }

        [Display(Name = "Слика")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "Име и презиме")]
        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        [Display(Name = "Прв професор")]
        public ICollection<Course> Courses { get; set; }

        [Display(Name = "Втор професор")]
        public ICollection<Course> CoursesSecond { get; set; }


    }
}
