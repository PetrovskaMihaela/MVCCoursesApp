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
    public class TeachersController : Controller
    {
        private readonly UniCoursesAppContext _context;
        private readonly IHostingEnvironment webHostEnvironment;
        public TeachersController(UniCoursesAppContext context, IHostingEnvironment hostingEnviroment)
        {
            _context = context;
            webHostEnvironment = hostingEnviroment;
        }

        // GET: Teachers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string teacherDegree, string teacherRank, string searchString)
        {
            IQueryable<Teacher> teachers = _context.Teacher.AsQueryable();
            IQueryable<string> degreeQuery = _context.Teacher.OrderBy(t => t.Degree).Select(t => t.Degree).Distinct();
            IQueryable<string> rankQuery = _context.Teacher.OrderBy(t => t.AcademicRank).Select(t => t.AcademicRank).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(t => t.FirstName.Contains(searchString) || t.LastName.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(teacherDegree))
            {
                teachers = teachers.Where(x => x.Degree == teacherDegree);
            }
            if (!string.IsNullOrEmpty(teacherRank))
            {
                teachers = teachers.Where(x => x.AcademicRank == teacherRank);
            }

            teachers = teachers.Include(t => t.Courses).Include(t => t.CoursesSecond);

            var teacherNameDegreeRankVM = new TeacherNameDegreeRankViewModel
            {
                DegreeVm = new SelectList(await degreeQuery.ToListAsync()),
                RankVm = new SelectList(await rankQuery.ToListAsync()),
                TeachersVm = await teachers.ToListAsync()
            };

            return View(teacherNameDegreeRankVM);
        }

        // GET: Teachers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(t => t.Courses)
                .Include(t => t.CoursesSecond)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        /*// POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }*/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Teacher teacher = new Teacher
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Degree = model.Degree,
                    AcademicRank = model.AcademicRank,
                    OfficeNumber = model.OfficeNumber,
                    HireDate = model.HireDate,
                    ProfilePicture = uniqueFileName,
                };

                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = teacher.Id });
            }
            return View();
        }

        // GET: Teachers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile imageUrl, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate,ProfilePicture")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }
            TeachersController uploadImage = new TeachersController(_context, webHostEnvironment);
            teacher.ProfilePicture = uploadImage.UploadedFile(imageUrl);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = teacher.Id });
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.Id == id);
        }

        private string UploadedFile(TeacherCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.ProfilePicture != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfilePicture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePicture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        //GET: Teachers/TeacherList
        public async Task<IActionResult> TeacherList()
        {
            var universityContext = _context.Teacher.Include(t => t.Courses).Include(t => t.CoursesSecond);
            return View(await universityContext.ToListAsync());
        }

        //GET: Teachers/TeacherViewDetails/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherViewDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(t => t.Courses)
                .Include(t => t.CoursesSecond)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        //Overloaded function UploadedFile for Edit
        public string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
