using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Data;
using StudentManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentManagement.Controllers
{
    [Authorize]
    public class GradesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToListAsync();
            return View(grades);
        }

        public IActionResult Create()
        {
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FullName");
            ViewBag.SubjectId = new SelectList(_context.Subjects, "Id", "SubjectName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Grade @grade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@grade);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Student added successfully!";
                TempData["MessageType"] = "success";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FullName", grade.StudentId);
            ViewBag.SubjectId = new SelectList(_context.Subjects, "Id", "SubjectName", grade.SubjectId);
            return View(@grade);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var grade = await _context.Grades.FindAsync(id);
            if (grade == null) return NotFound();
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FullName");
            ViewBag.SubjectId = new SelectList(_context.Subjects, "Id", "SubjectName");
            return View(grade);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Grade grade)
        {
            if (id != grade.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Grade updated successfully!";
                    TempData["MessageType"] = "success";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.Id))
                    {
                        TempData["Message"] = "Grade not found.";
                        TempData["MessageType"] = "error";
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StudentId = new SelectList(_context.Students, "Id", "FullName", grade.StudentId);
            ViewBag.SubjectId = new SelectList(_context.Subjects, "Id", "SubjectName", grade.SubjectId);
            return View(grade);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}
