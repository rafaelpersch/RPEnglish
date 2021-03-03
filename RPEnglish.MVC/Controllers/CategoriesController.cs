using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPEnglish.MVC.DatabaseContext;
using RPEnglish.MVC.Entities;
using RPEnglish.MVC.Tools;

namespace RPEnglish.MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly MyDbContext _context;

        public CategoriesController(MyDbContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public async Task<IActionResult> Index()
        {
            if (TempData["ApplicationMessage"] != null)
            {
                ViewBag.ApplicationMessage = TempData["ApplicationMessage"].ToString();
            }

            ViewBag.Categories = await _context.Categories.OrderBy(x=> x.Name).ToListAsync();
            return View();
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public IActionResult Create()
        {
            return View("Register", new Category());
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            try
            {
                category.Id = Guid.NewGuid();
                category.Validate();
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["ApplicationMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            
            if (category == null)
            {
                return RedirectToAction("Index");
            }

            return View("Register", category);
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            try
            {
                if (id != category.Id)
                {
                    return RedirectToAction("Index");
                }

                category.Validate();

                var categorySaved = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (categorySaved == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Categories.Update(category);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["ApplicationMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    return RedirectToAction("Index");
                }

                var words = await _context.Words.FirstOrDefaultAsync(x => x.CategoryId == id);

                if (words != null)
                {
                    throw new ApplicationException("Category used!");
                }

                _context.Categories.Remove(category);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                TempData["ApplicationMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}