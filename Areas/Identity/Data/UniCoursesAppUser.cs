using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UniCoursesApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the UniCoursesAppUser class
    public class UniCoursesAppUser : IdentityUser
    {
        public int? StudentId { get; set; }

        public int? TeacherId { get; set; }
    }
}
