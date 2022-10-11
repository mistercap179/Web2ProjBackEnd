using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Conversions;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class ProizvodController : ControllerBase
    {
        private readonly IConversions conversions;
        private readonly DostavaContext _context;

        public ProizvodController(DostavaContext context,IConversions _conversions)
        {
            _context = context;
            conversions = _conversions;
        }

        // GET: private loginUrl = "https://localhost:5001/api/Osoba/Login"
       
        [HttpGet]
        //[Authorize(Roles = "Dostavljac")]
        public async Task<ActionResult<IEnumerable<Proizvod>>> GetProizvod()
        {
            List<Models_Backend.Proizvod> proizvodi = new List<Models_Backend.Proizvod>();
            
            await _context.Proizvod.ForEachAsync(x => proizvodi.Add(conversions.ConversionProizvod(x)));
            
            return Ok(proizvodi);
        }

        // GET: api/Proizvod/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Proizvod>> GetProizvod(int id)
        {
            Models.Proizvod proizvod = await _context.Proizvod.FirstOrDefaultAsync(x=>x.IdProizvod == id);
           
            Models_Backend.Proizvod returnProizvod =  conversions.ConversionProizvod(proizvod);

            if (proizvod == null)
            {
                return NotFound();
            }

            return Ok(returnProizvod);
        }

        // PUT: api/Proizvod/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProizvod(int id, Proizvod proizvod)
        {
            if (id != proizvod.IdProizvod)
            {
                return BadRequest();
            }

            _context.Entry(proizvod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProizvodExists(id))
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

        // POST: api/Proizvod
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Proizvod>> PostProizvod(object proiz)
        {
            Proizvod proizvod = JsonConvert.DeserializeObject<Proizvod>(proiz.ToString());

            lock (_context)
            {
                _context.Proizvod.Add(proizvod);
                _context.SaveChanges();
            }

            return CreatedAtAction("GetProizvod", conversions.ConversionProizvod(proizvod));
        }

        // DELETE: api/Proizvod/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Proizvod>> DeleteProizvod(int id)
        {
            var proizvod = await _context.Proizvod.FindAsync(id);
            if (proizvod == null)
            {
                return NotFound();
            }

            _context.Proizvod.Remove(proizvod);
            await _context.SaveChangesAsync();

            return proizvod;
        }

        private bool ProizvodExists(int id)
        {
            return _context.Proizvod.Any(e => e.IdProizvod == id);
        }
    }
}
