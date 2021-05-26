using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniCoursesApp.Models;
using UniCoursesApp.ViewModels;

namespace UniCoursesApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UniCoursesAppContext _context;
        private readonly IHostingEnvironment webHostingEnvironment;

        public StudentsController(UniCoursesAppContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            webHostingEnvironment = hostingEnvironment;
        }

        // GET: Students
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string StudentIndex, string SearchString)
        {
            IQueryable<Student> students = _context.Student.AsQueryable();
            IQueryable<string> IndexQuery = _context.Student.OrderBy(s => s.StudentId).Select(s => s.StudentId).Distinct();

            if (!string.IsNullOrEmpty(SearchString))
            {
                students = students.Where(t => t.FirstName.Contains(SearchString) || t.LastName.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(StudentIndex))
            {
                students = students.Where(x => x.StudentId == StudentIndex);
            }

            students = students.Include(s => s.Courses).ThenInclude(e => e.Course);

            var studentNameIndexVM = new StudentNameIndexViewModel
            {
                IndexVm = new SelectList(await IndexQuery.ToListAsync()),
                StudentVm = await students.ToListAsync()
            };

            return View(studentNameIndexVM);
        }

        // GET: Students/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .Include(s => s.Courses)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        /* // POST: Students/Create
         // To protect from overposting attacks, enable the specific properties you want to bind to, for 
         // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create([Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel")] Student student)
         {
             if (ModelState.IsValid)
             {
                 _context.Add(student);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             }
             return View(student);
         }*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Student student = new Student
                {
                    Id = model.Id,
                    StudentId = model.StudentId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EnrollmentDate = model.EnrollmentDate,
                    AcquiredCredits = model.AcquiredCredits,
                    CurrentSemester = model.CurrentSemester,
                    EducationLevel = model.EducationLevel,
                    ProfilePicture = uniqueFileName
                };

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id = student.Id });
            }
            return View();
        }

        private string UploadedFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "studentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Students/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, StudentCreateViewModel model, [Bind("Id,StudentId,FirstName,LastName,EnrollmentDate,AcquiredCredits,CurrentSemester,EducationLevel,ProfilePicture")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            StudentsController uploadImage = new StudentsController(_context, webHostingEnvironment);
            student.ProfilePicture = uploadImage.UploadedFile(imageUrl);


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = student.Id });
            }
            return View(student);
        }

        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "studentImages");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/StudentList
        [Authorize]
        public async Task<IActionResult> StudentList()
        {
            var universityContext = _context.Student.Include(s => s.Courses).ThenInclude(e => e.Course);
            return View(await universityContext.ToListAsync());
        }

        // GET: Students/StudentViewDetails/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
            .Include(s => s.Courses)
            .ThenInclude(e => e.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
