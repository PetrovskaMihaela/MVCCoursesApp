using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoursesApp.Models
{
    public class User
    {
        [Required]
        [Display(Name = "Корисничко име")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Емаил адреса")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Лозинка")]
        public string Password { get; set; }
        [Display(Name = "Професор")]
        public int? TeacherId { get; set; }
        [Display(Name = "Студент")]
        public int? StudentId { get; set; }
    }
}
