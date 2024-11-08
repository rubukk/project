using AuthSystem.Areas.Identity.Data;
using AuthSystem.Models;
using AuthSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.Extensions.Logging;


namespace AuthSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;


        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;

        }

        public IActionResult Index()
        {
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            // Kullanıcının kimliğini al
            var userId = _userManager.GetUserId(User);

            // Kullanıcının notlarını al
            var userGrades = _context.CourseGrades
                .Where(cg => cg.UserId == userId)
                .ToList();

            // Notları view'a gönder
            return View(userGrades);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}