using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniCoursesApp.Models;
using UniCoursesApp.ViewModels;

namespace UniCoursesApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly UniCoursesAppContext _context;

        public CoursesController(UniCoursesAppContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index( int courseSem, string courseProg, string searchString)
        {
            IQueryable<Course> courses = _context.Course.AsQueryable();
            // IQueryable<String> titleQuery = _context.Course.OrderBy(m => m.Title).Select(m => m.Title).Distinct();
            IQueryable<int> semesterQuery = _context.Course.OrderBy(c => c.Semester).Select(c => c.Semester).Distinct();
            IQueryable<string> ProgrammeQuery = _context.Course.OrderBy(c => c.Programme).Select(c => c.Programme).Distinct();

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(s => s.Title.Contains(searchString));
            }
            if(courseSem > 0)
            {
                courses = courses.Where(x => x.Semester == courseSem);
            }
            if (!string.IsNullOrEmpty(courseProg))
            {
                courses = courses.Where(x => x.Programme == courseProg);
            }

            courses = courses.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student);

            var courseTitleSemProgVM = new CourseTitleSemProgViewModel
            {
                SemesterVm = new SelectList(await semesterQuery.ToListAsync()),
                ProgrammeVm = new SelectList(await ProgrammeQuery.ToListAsync()), 
                CoursesVm = await courses.ToListAsync()
            };

            return View(courseTitleSemProgVM);

            /*var uniCoursesAppContext = _context.Course.Include(c => c.FirstTeacher).Include(c => c.SecondTeacher).Include(c => c.Students).ThenInclude(c => c.Student);
            return View(await uniCoursesAppContext.ToListAsync());*/
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Credits,Semester,Programme,EducationLevel,FirstTeacherId,SecondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = _context.Course.Where(c => c.Id == id).Include(c => c.Students).First();


            //var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }


            EnrollmentViewModel viewmodel = new EnrollmentViewModel
            {
                Course = course,
                StudentList = new MultiSelectList(_context.Student.OrderBy(s => s.FirstName), "Id", "FullName"),
                SelectedStudents =course.Students.Select(e => e.StudentId)
            };
            //CoursesVm = await courses.ToListAsync()

            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", course.SecondTeacherId);
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EnrollmentViewModel viewmodel)
        {
            if (id != viewmodel.Course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Course);
                    await _context.SaveChangesAsync();

                    IEnumerable<int> ListStudents = viewmodel.SelectedStudents;
                    IQueryable<Enrollment> toBeRemoved = _context.Enrollment.Where(s => !ListStudents.Contains(s.StudentId) && s.CourseId == id);
                    _context.Enrollment.RemoveRange(toBeRemoved);

                    IEnumerable<int> existStudents = (IEnumerable<int>)_context.Enrollment.Where(s => ListStudents.Contains(s.StudentId) && s.CourseId == id).Select(s => s.StudentId);
                    IEnumerable<int> newStudents = ListStudents.Where(s => !existStudents.Contains(s));
                    foreach (int studentId in newStudents)
                        _context.Enrollment.Add(new Enrollment { StudentId = studentId, CourseId = id });

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewmodel.Course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FirstTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", viewmodel.Course.FirstTeacherId);
            ViewData["SecondTeacherId"] = new SelectList(_context.Teacher, "Id", "FullName", viewmodel.Course.SecondTeacherId);
            return View(viewmodel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.FirstTeacher)
                .Include(c => c.SecondTeacher)
                .Include(c => c.Students).ThenInclude(c => c.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
