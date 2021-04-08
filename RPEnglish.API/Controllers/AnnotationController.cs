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
    public class AnnotationController : ControllerBase
    {
        private readonly MyDbContext myDbContext;
        public AnnotationController([FromServices] MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Annotation>>> Get()
        {
            try
            {
                return await myDbContext.Annotations.OrderBy(x => x.Text).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Annotation>> Get(Guid id)
        {
            try
            {
                var annotation = await myDbContext.Annotations.FindAsync(id);

                if (annotation == null)
                {
                    return NotFound();
                }

                return annotation;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Annotation annotation)
        {
            try
            {
                if (annotation == null)
                {
                    throw new ApplicationException("Object annotation null");
                }

                if (id != annotation.Id)
                {
                    return BadRequest();
                }

                annotation.Validate();

                var annotationSaved = await myDbContext.Annotations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (annotationSaved == null)
                {
                    return NotFound();
                }

                myDbContext.Entry(annotation).State = EntityState.Modified;

                await myDbContext.SaveChangesAsync();

                return NoContent();                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        public async Task<ActionResult<Annotation>> Post(Annotation annotation)
        {
            try
            {
                if (annotation == null)
                {
                    throw new ApplicationException("Object annotation null");
                }

                annotation.Id = Guid.NewGuid();

                annotation.Validate();

                myDbContext.Annotations.Add(annotation);

                await myDbContext.SaveChangesAsync();

                return CreatedAtAction("Get", new { id = annotation.Id }, annotation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Annotation>> Delete(Guid id)
        {
            try
            {
                var annotation = await myDbContext.Annotations.FindAsync(id);

                if (annotation == null)
                {
                    return NotFound();
                }

                myDbContext.Annotations.Remove(annotation);

                await myDbContext.SaveChangesAsync();

                return annotation;                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }
    }
}
