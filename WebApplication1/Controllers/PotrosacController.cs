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
    public class PotrosacController : ControllerBase
    {
        private readonly IConversions conversions;
        private readonly DostavaContext _context;

        public PotrosacController(DostavaContext context,IConversions _conversions)
        {
            _context = context;
            conversions = _conversions;
    }

        // GET: api/Potrosac
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Potrosac>>> GetPotrosac()
        {
            List<Models_Backend.Potrosac> potrosaci = new List<Models_Backend.Potrosac>();
            await _context.Potrosac.Include(x=>x.Porudzbina).Include(x=>x.IdPotrosacNavigation).
                ForEachAsync(x=>potrosaci.Add(conversions.ConversionPotrosac(x)));
            return Ok(potrosaci);
        }

        // GET: api/Potrosac/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Potrosac>> GetPotrosac(int id)
        {
            Models_Backend.Potrosac potrosac = conversions.ConversionPotrosac(await _context.Potrosac.
                Include(x=>x.IdPotrosacNavigation).Include(x=>x.Porudzbina).FirstOrDefaultAsync(x=>x.IdPotrosac == id));

            if (potrosac == null)
            {
                return NotFound();
            }

            return Ok(potrosac);
        }

        // PUT: api/Potrosac/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPotrosac(int id, Potrosac potrosac)
        {
            if (id != potrosac.IdPotrosac)
            {
                return BadRequest();
            }

            _context.Entry(potrosac).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PotrosacExists(id))
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

        // POST: api/Potrosac
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Potrosac>> PostPotrosac(Potrosac potrosac)
        {
            _context.Potrosac.Add(potrosac);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PotrosacExists(potrosac.IdPotrosac))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPotrosac", new { id = potrosac.IdPotrosac }, potrosac);
        }

        // DELETE: api/Potrosac/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Potrosac>> DeletePotrosac(int id)
        {
            var potrosac = await _context.Potrosac.FindAsync(id);
            if (potrosac == null)
            {
                return NotFound();
            }

            _context.Potrosac.Remove(potrosac);
            await _context.SaveChangesAsync();

            return potrosac;
        }

        private bool PotrosacExists(int id)
        {
            return _context.Potrosac.Any(e => e.IdPotrosac == id);
        }
    }
}
