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
    public class AnnotationsController : Controller
    {
        private readonly MyDbContext _context;

        public AnnotationsController(MyDbContext context)
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

            ViewBag.Annotations = await _context.Annotations.OrderBy(x => x.Text).ToListAsync();

            return View();
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public IActionResult Create()
        {
            return View("Register", new Annotation());
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Annotation annotation)
        {
            try 
            {
                annotation.Id = Guid.NewGuid();
                annotation.Validate();
                _context.Annotations.Add(annotation);
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
            var annotation = await _context.Annotations.FindAsync(id);
            
            if (annotation == null)
            {
                return RedirectToAction("Index");
            }

            return View("Register", annotation);
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Annotation annotation)
        {
            try
            {
                if (id != annotation.Id)
                {
                    return RedirectToAction("Index");
                }

                annotation.Validate();

                var annotationSaved = await _context.Annotations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (annotationSaved == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Annotations.Update(annotation);

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
                var annotation = await _context.Annotations.FindAsync(id);

                if (annotation == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Annotations.Remove(annotation);
                
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