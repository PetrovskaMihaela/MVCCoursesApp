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
    public class TeachersController : Controller
    {
        private readonly UniCoursesAppContext _context;

        public TeachersController(UniCoursesAppContext context)
        {
            _context = context;
        }

        // GET: Teachers
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
        public async Task<IActionResult> Details(int? id)
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

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
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
        }

        // GET: Teachers/Edit/5
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Degree,AcademicRank,OfficeNumber,HireDate")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
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
    }
}
