using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Models;
using AuthSystem.Data;
using Microsoft.AspNetCore.Identity;
using AuthSystem.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class GradeController : Controller
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GradeController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var grades = await _context.CourseGrades
                .Include(g => g.User)
                .Where(g => g.Grade != null && g.CourseName != null && g.FirstName != null)
                .ToListAsync();
            return View(grades);
        }

        public async Task<IActionResult> Create()
        {
            var students = await _userManager.Users
                .Where(u => u.Role == "Student" && u.FirstName != null && u.LastName != null)
                .ToListAsync();

            ViewBag.Students = students;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseGrade courseGrade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Öğretmen bilgilerini al
                    var teacher = await _userManager.GetUserAsync(User);
                    if (teacher == null || string.IsNullOrEmpty(teacher.FirstName) || string.IsNullOrEmpty(teacher.LastName))
                    {
                        ModelState.AddModelError("", "Teacher information is incomplete or not found.");
                        return View(courseGrade);
                    }

                    // Öğrenci bilgilerini al
                    var student = await _userManager.FindByIdAsync(courseGrade.UserId);
                    if (student == null || string.IsNullOrEmpty(student.FirstName))
                    {
                        ModelState.AddModelError("", "Student information is incomplete or not found.");
                        return View(courseGrade);
                    }

                    // Null kontrolü yaparak değerleri ata
                    courseGrade.TeachersName = $"{teacher.FirstName.Trim()} {teacher.LastName.Trim()}".Trim();
                    courseGrade.FirstName = student.FirstName.Trim();
                    courseGrade.Grade = (courseGrade.Grade ?? "").Trim();
                    courseGrade.CourseName = (courseGrade.CourseName ?? "").Trim();

                    // Boş string kontrolü
                    if (string.IsNullOrWhiteSpace(courseGrade.Grade))
                    {
                        ModelState.AddModelError("Grade", "Grade cannot be empty.");
                        return View(courseGrade);
                    }

                    if (string.IsNullOrWhiteSpace(courseGrade.CourseName))
                    {
                        ModelState.AddModelError("CourseName", "Course name cannot be empty.");
                        return View(courseGrade);
                    }

                    _context.CourseGrades.Add(courseGrade);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error occurred while adding grade: {ex.Message}");
                }
            }

            // Hata durumunda öğrenci listesini yeniden yükle
            ViewBag.Students = await _userManager.Users
                .Where(u => u.Role == "Student" && u.FirstName != null && u.LastName != null)
                .ToListAsync();

            return View(courseGrade);
        }
    }
}