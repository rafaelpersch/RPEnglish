using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPEnglish.MVC.DatabaseContext;
using RPEnglish.MVC.Entities;
using RPEnglish.MVC.Tools;

namespace RPEnglish.MVC.Controllers
{
    public class WordsController : Controller
    {
        private readonly MyDbContext _context;

        public WordsController(MyDbContext context)
        {
            _context = context;
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public async Task<IActionResult> Index(Guid categoryid)
        {
            if (TempData["ApplicationMessage"] != null)
            {
                ViewBag.ApplicationMessage = TempData["ApplicationMessage"].ToString();
            }

            if (categoryid != Guid.Empty)
            {
                ViewBag.Words = await _context.Words.Include(x => x.Category).Where(x =>x.CategoryId == categoryid).OrderBy(x=> x.Name).ToListAsync();
            }
            else
            {
                ViewBag.Words = new List<Word>();
            }

            ViewBag.Categoryid = categoryid;
            ViewBag.Categories = await _context.Categories.OrderBy(x => x.Name).ToListAsync();

            return View();
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.OrderBy(x => x.Name).ToListAsync();
            return View("Register", new Word());
        }
        
        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Word word)
        {
            try
            {
                word.Id = Guid.NewGuid();
                word.Validate();
                _context.Words.Add(word);
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
            var word = await _context.Words.FindAsync(id);
            
            if (word == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.Categories = await _context.Categories.OrderBy(x => x.Name).ToListAsync();

            return View("Register", word);
        }

        [ServiceFilter(typeof(CustomAuthorizationFilter))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Word word)
        {
            try
            {
                if (id != word.Id)
                {
                    return RedirectToAction("Index");
                }

                word.Validate();

                var wordSaved = await _context.Words.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (wordSaved == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Words.Update(word);

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
                var word = await _context.Words.FindAsync(id);

                if (word == null)
                {
                    return RedirectToAction("Index");
                }

                _context.Words.Remove(word);

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