using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UniCoursesApp.Areas.Identity.Data;
using UniCoursesApp.Models;

namespace UniCoursesApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UniCoursesAppContext _context;
        private readonly UserManager<UniCoursesAppUser> userManager;
        public HomeController(ILogger<HomeController> logger, UniCoursesAppContext context, UserManager<UniCoursesAppUser> usrMgr)
        {
            _logger = logger;
            _context = context;
            userManager = usrMgr;
        }

        public async Task<IActionResult> Index()
        {
            //return View();
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Courses");
            }
            else if (User.IsInRole("Teacher"))
            {
               
                var userID = userManager.GetUserId(User);
                UniCoursesAppUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("CoursesByTeacher", "Courses", new { id = user.TeacherId });
            }
            else if (User.IsInRole("Student"))
            {
                var userID = userManager.GetUserId(User);
                UniCoursesAppUser user = await userManager.FindByIdAsync(userID);
                return RedirectToAction("MyEnrollments", "Enrollments", new { id = user.StudentId });
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
