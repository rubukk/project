using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AuthSystem.Data;
using AuthSystem.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly AuthDbContext _context;

        public DashboardModel(AuthDbContext context)
        {
            _context = context;
        }

        public List<CourseGrade> CourseGrades { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.Users.FindAsync(userId);

            if (user != null)
            {
                CourseGrades = await _context.CourseGrades
                    .Where(cg => cg.UserId == userId && cg.FirstName == user.FirstName)
                    .ToListAsync();
            }
            else
            {
                CourseGrades = new List<CourseGrade>();
            }

            return Page();
        }
    }

}