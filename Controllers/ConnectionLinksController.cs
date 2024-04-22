using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThisLinq.Data;
using ThisLinq.Models;

namespace ThisLinq.Controllers
{
    public class ConnectionLinksController : Controller
    {
        private readonly Linq22DbContext _context;

        public ConnectionLinksController(Linq22DbContext context)
        {
            _context = context;
        }
        //Edit SubjectName
        public async Task<IActionResult> EditSubjectName(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Courses = new SelectList(_context.Courses, "CourseId", "CourseName", course.CourseId);
            return View(course);
        }
        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSubjectName(int id, [Bind("CourseId,CourseName")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCourse = await _context.Courses.FindAsync(course.CourseId);
                if (existingCourse == null)
                {
                    return NotFound();
                }

                existingCourse.CourseName = course.CourseName;
                _context.Update(existingCourse);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "ConnectionLinks");
            }
            return View(course);
        }

        //Students that read programming1 and their teacher!
        public IActionResult TeachingProgramming()
        {
            try
            {
                // Hämta lärare och elever som är kopplade till kursen 'Programming 1'
                var teachersAndStudentsTeachingProgramming = _context.Teachers
                    .Join(_context.Connections,
                        teacher => teacher.TeacherId,
                        connection => connection.FK_TeacherId,
                        (teacher, connection) => new { Teacher = teacher, Connection = connection })
                    .Join(_context.Students,
                        ts => ts.Connection.FK_StudentId,
                        student => student.StudentId,
                        (ts, student) => new { ts.Teacher, ts.Connection, Student = student })
                    .Where(join => join.Connection.Courses.CourseName == "Programmering 1")
                    .Select(join => new
                    {
                        Teacher = join.Teacher,
                        Student = join.Student
                    })
                    .Distinct()
                    .ToList();

                return View(teachersAndStudentsTeachingProgramming);
            }
            catch (Exception ex)
            {
                // Logga eventuellt fel
                Console.WriteLine(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }


        //Teacher by subject
        public async Task<IActionResult> GetTeacherBySubject(string searchString = "")
        {
            var teacherResult = await (from r in _context.Connections
                                       join t in _context.Teachers on r.FK_TeacherId equals t.TeacherId
                                       join c in _context.Courses on r.FK_CourseId equals c.CourseId
                                       where c.CourseName.Contains(searchString)
                                       orderby r.Id
                                       select new TeacherCourseViewModel
                                       {
                                           Teacher = t,
                                           CourseName = c.CourseName,
                                           RelationshipId = r.Id
                                       }).ToListAsync();

            return View(teacherResult);
        }

        //Change Teacher
        public async Task<IActionResult> ChangeTeacherForProgramming1()
        {
            var teachers = await _context.Teachers.ToListAsync();
            return View(teachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeTeacherForProgramming1(int? newTeacherId)
        {
            if (newTeacherId == null)
            {
                // Om newTeacherId är null, omdirigera tillbaka till vyn med ett felmeddelande
                TempData["ErrorMessage"] = "Please select a new teacher.";
                return RedirectToAction("ChangeTeacherForProgramming1");
            }
            var programming1Course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseName == "Programmering 1");
            if (programming1Course == null)
            {
                return NotFound();
            }
            var currentTeacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Connections.Any(c => c.Courses.CourseId == programming1Course.CourseId));
            if (currentTeacher == null)
            {
                return NotFound();
            }
            var newTeacher = await _context.Teachers.FindAsync((int)newTeacherId);
            if (newTeacher == null)
            {
                return NotFound();
            }
            currentTeacher.Connections.First(c => c.Courses.CourseId == programming1Course.CourseId).FK_TeacherId = (int)newTeacherId;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "ConnectionLinks");
        }

        // GET: ConnectionLinks
        public async Task<IActionResult> Index()
        {
            var linq22DbContext = _context.Connections.Include(c => c.Classes).Include(c => c.Courses).Include(c => c.Students).Include(c => c.Teachers);
            return View(await linq22DbContext.ToListAsync());
        }

        // GET: ConnectionLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionLink = await _context.Connections
                .Include(c => c.Classes)
                .Include(c => c.Courses)
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectionLink == null)
            {
                return NotFound();
            }

            return View(connectionLink);
        }

        // GET: ConnectionLinks/Create
        public IActionResult Create()
        {
            ViewData["FK_ClassId"] = new SelectList(_context.Classes, "ClassID", "ClassName");
            ViewData["FK_CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName");
            ViewData["FK_StudentId"] = new SelectList(_context.Students, "StudentId", "FirstName");
            ViewData["FK_TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName");
            return View();
        }

        // POST: ConnectionLinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FK_ClassId,FK_CourseId,FK_StudentId,FK_TeacherId")] ConnectionLink connectionLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(connectionLink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FK_ClassId"] = new SelectList(_context.Classes, "ClassID", "ClassName", connectionLink.FK_ClassId);
            ViewData["FK_CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", connectionLink.FK_CourseId);
            ViewData["FK_StudentId"] = new SelectList(_context.Students, "StudentId", "FirstName", connectionLink.FK_StudentId);
            ViewData["FK_TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", connectionLink.FK_TeacherId);
            return View(connectionLink);
        }

        // GET: ConnectionLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionLink = await _context.Connections.FindAsync(id);
            if (connectionLink == null)
            {
                return NotFound();
            }
            ViewData["FK_ClassId"] = new SelectList(_context.Classes, "ClassID", "ClassName", connectionLink.FK_ClassId);
            ViewData["FK_CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", connectionLink.FK_CourseId);
            ViewData["FK_StudentId"] = new SelectList(_context.Students, "StudentId", "FirstName", connectionLink.FK_StudentId);
            ViewData["FK_TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", connectionLink.FK_TeacherId);
            return View(connectionLink);
        }

        // POST: ConnectionLinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FK_ClassId,FK_CourseId,FK_StudentId,FK_TeacherId")] ConnectionLink connectionLink)
        {
            if (id != connectionLink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(connectionLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConnectionLinkExists(connectionLink.Id))
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
            ViewData["FK_ClassId"] = new SelectList(_context.Classes, "ClassID", "ClassName", connectionLink.FK_ClassId);
            ViewData["FK_CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName", connectionLink.FK_CourseId);
            ViewData["FK_StudentId"] = new SelectList(_context.Students, "StudentId", "FirstName", connectionLink.FK_StudentId);
            ViewData["FK_TeacherId"] = new SelectList(_context.Teachers, "TeacherId", "FirstName", connectionLink.FK_TeacherId);
            return View(connectionLink);
        }

        // GET: ConnectionLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var connectionLink = await _context.Connections
                .Include(c => c.Classes)
                .Include(c => c.Courses)
                .Include(c => c.Students)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (connectionLink == null)
            {
                return NotFound();
            }

            return View(connectionLink);
        }

        // POST: ConnectionLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var connectionLink = await _context.Connections.FindAsync(id);
            if (connectionLink != null)
            {
                _context.Connections.Remove(connectionLink);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConnectionLinkExists(int id)
        {
            return _context.Connections.Any(e => e.Id == id);
        }
    }
}
