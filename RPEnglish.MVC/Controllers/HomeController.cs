using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RPEnglish.MVC.DatabaseContext;
using RPEnglish.MVC.Models;
using RPEnglish.MVC.Tools;

namespace RPEnglish.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _context;

        public HomeController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var user = SessionManagement.GetSession(HttpContext);

            if (user != null)
            {
                return RedirectToAction("Main");
            }

            if (TempData["ApplicationMessage"] != null)
            {
                ViewBag.ApplicationMessage = TempData["ApplicationMessage"].ToString();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    throw new ApplicationException("Incorrect data!");
                }

                var userPassword = await _context.UsersPassword.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Email == email && x.Password == Cryptography.Encrypt(email + password));

                if (userPassword == null)
                {
                    throw new ApplicationException("Invalid login!");
                }

                SessionManagement.CreateSession(HttpContext, userPassword.User);

                return RedirectToAction("Index", "Annotations");
            }
            catch (Exception ex)
            {
                TempData["ApplicationMessage"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            SessionManagement.DeleteSession(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
