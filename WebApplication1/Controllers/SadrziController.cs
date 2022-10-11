using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Conversions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SadrziController : ControllerBase
    {
        private readonly IConversions conversions;
        private readonly DostavaContext _context;

        public SadrziController(DostavaContext context,IConversions _conversions)
        {
            _context = context;
            conversions = _conversions;
        }

        // GET: api/Sadrzi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sadrzi>>> GetSadrzi()
        {
            return await _context.Sadrzi.ToListAsync();
        }

        // GET: api/Sadrzi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sadrzi>> GetSadrzi(int id)
        {
            var sadrzi = await _context.Sadrzi.FindAsync(id);

            if (sadrzi == null)
            {
                return NotFound();
            }

            return sadrzi;
        }

        // PUT: api/Sadrzi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSadrzi(int id, Sadrzi sadrzi)
        {
            if (id != sadrzi.IdSadrzi)
            {
                return BadRequest();
            }

            _context.Entry(sadrzi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SadrziExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sadrzi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Sadrzi>> PostSadrzi(Sadrzi sadrzi)
        {
            _context.Sadrzi.Add(sadrzi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSadrzi", new { id = sadrzi.IdSadrzi }, sadrzi);
        }

        // DELETE: api/Sadrzi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Sadrzi>> DeleteSadrzi(int id)
        {
            var sadrzi = await _context.Sadrzi.FindAsync(id);
            if (sadrzi == null)
            {
                return NotFound();
            }

            _context.Sadrzi.Remove(sadrzi);
            await _context.SaveChangesAsync();

            return sadrzi;
        }

        private bool SadrziExists(int id)
        {
            return _context.Sadrzi.Any(e => e.IdSadrzi == id);
        }
    }
}
