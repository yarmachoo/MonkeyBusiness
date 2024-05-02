using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonkeyBuisness.DAL;
using MonkeyBuisness.Models.Entity;
using MonkeyBuisness.Models.ViewModels.Person;
using MonkeyBuisness.Service.Implementations;
using MonkeyBuisness.Service.Interfaces;
using System.Security.Claims;

namespace MonkeyBuisness.Controllers
{
    public class AccountController : Controller
    {
        private readonly  AppDbContext db;

        public AccountController(AppDbContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                PersonEntity person = await db.People.FirstOrDefaultAsync(p => p.Login == model.Login && p.Password == model.Password);
                if(person != null)
                {
                    await Authenticate(model.Login);
                    return RedirectToAction("Index", "Task");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                PersonEntity person = await db.People.FirstOrDefaultAsync(p => p.Login == model.Login);
                if (person == null)
                {
                    db.People.Add(new PersonEntity { Login = model.Login, Password = model.Password });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Login);
                    return RedirectToAction("Index", "Task");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
