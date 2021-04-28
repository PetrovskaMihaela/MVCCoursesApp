using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoursesApp.Models
{
    public class Teacher
    {
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
        public string Degree { get; set; }

        [StringLength(25)]
        [Display(Name = "Звање")]
        public string AcademicRank { get; set; }

        [StringLength(50)]
        [Display(Name = "Канцеларија")]
        public string OfficeNumber { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Датум на вработување")]
        public DateTime? HireDate { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        public ICollection<Course> Courses { get; set; }
        public ICollection<Course> CoursesSecond { get; set; }
    }
}