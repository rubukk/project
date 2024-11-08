using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Models;
using AuthSystem.Data;
using Microsoft.EntityFrameworkCore;
using AuthSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace AuthSystem.Controllers
{
    public class TeacherController : Controller
    {
        private readonly UserManager<Teacher> _userManager;
        private readonly SignInManager<Teacher> _signInManager;

        public TeacherController(UserManager<Teacher> userManager, SignInManager<Teacher> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Öğretmen Kayıt Sayfası
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(TeacherRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var teacher = new Teacher
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await _userManager.CreateAsync(teacher, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Teacher");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // Öğretmen Giriş Sayfası
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(TeacherLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt.");
            }
            return View(model);
        }
    }
}