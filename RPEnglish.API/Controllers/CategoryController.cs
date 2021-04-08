using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RPEnglish.API.DatabaseContext;
using RPEnglish.API.Entities;

namespace RPEnglish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        public CategoryController([FromServices] MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            try
            {
                return await myDbContext.Categories.OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> Get(Guid id)
        {
            try
            {
                var category = await myDbContext.Categories.FindAsync(id);

                if (category == null)
                {
                    return NotFound();
                }

                return category;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new ApplicationException("Object category null");
                }

                if (id != category.Id)
                {
                    return BadRequest();
                }

                category.Validate();

                var categorySaved = await myDbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (categorySaved == null)
                {
                    return NotFound();
                }

                myDbContext.Entry(category).State = EntityState.Modified;

                await myDbContext.SaveChangesAsync();

                return NoContent();                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Post(Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new ApplicationException("Object category null");
                }

                category.Id = Guid.NewGuid();

                category.Validate();

                myDbContext.Categories.Add(category);

                await myDbContext.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = category.Id }, category);                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> Delete(Guid id)
        {
            try
            {
                var category = await myDbContext.Categories.FindAsync(id);
                
                if (category == null)
                {
                    return NotFound();
                }

                var words = await myDbContext.Words.FirstOrDefaultAsync(x => x.CategoryId == id);

                if (words != null)
                {
                    throw new ApplicationException("Category used!");
                }

                myDbContext.Categories.Remove(category);
                
                await myDbContext.SaveChangesAsync();

                return category;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}