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
    public class WordController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        public WordController([FromServices] MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Word>>> Get()
        {
            try
            {
                return await myDbContext.Words.Include(x => x.Category).OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetByCategory/{categoryid}")]
        public async Task<ActionResult<IEnumerable<Word>>> GetByCategory(Guid categoryid)
        {
            try
            {
                return await myDbContext.Words.Include(x => x.Category).Where(x => x.CategoryId == categoryid).OrderBy(x => x.Name).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> Get(Guid id)
        {
            try
            {
                var word = await myDbContext.Words.FindAsync(id);

                if (word == null)
                {
                    return NotFound();
                }

                return word;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Word word)
        {
            try
            {
                if (word == null)
                {
                    throw new ApplicationException("Object word null");
                }

                if (id != word.Id)
                {
                    return BadRequest();
                }

                word.Validate();

                var wordSaved = await myDbContext.Words.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (wordSaved == null)
                {
                    return NotFound();
                }

                myDbContext.Entry(word).State = EntityState.Modified;

                await myDbContext.SaveChangesAsync();

                return NoContent();                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        public async Task<ActionResult<Word>> Post(Word word)
        {
            try
            {
                if (word == null)
                {
                    throw new ApplicationException("Object word null");
                }

                word.Id = Guid.NewGuid();

                word.Validate();

                myDbContext.Words.Add(word);

                await myDbContext.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = word.Id }, word);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Word>> Delete(Guid id)
        {
            try
            {
                var word = await myDbContext.Words.FindAsync(id);
                
                if (word == null)
                {
                    return NotFound();
                }

                myDbContext.Words.Remove(word);
                
                await myDbContext.SaveChangesAsync();

                return word;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }        
    }
}