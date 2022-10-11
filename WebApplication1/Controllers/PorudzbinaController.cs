using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class PorudzbinaController : ControllerBase
    {
        private readonly IConversions conversions;
        private readonly DostavaContext _context;
        public PorudzbinaController(DostavaContext context,IConversions _conversions)
        {
            _context = context;
            conversions = _conversions;
        }

        // GET: api/Porudzbina
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Porudzbina>>> GetPorudzbina()
        {
            
            List<Models_Backend.Porudzbina> returnPorudzbine = new List<Models_Backend.Porudzbina>();

            List<Models.Porudzbina> porudzbine = await _context.Porudzbina.Include(x=>x.IdpotrosacNavigation).Include(x=>x.IddostavljacNavigation).Include(x=>x.Sadrzi).ThenInclude(x=>x.IdproizvodNavigation).Where(x=>x.IddostavljacNavigation.StatusProfila == 1 || x.IddostavljacNavigation.StatusProfila == 2).ToListAsync();
            

            porudzbine.ForEach(x => returnPorudzbine.Add((conversions.ConversionPorudzbina(x))));

            return Ok(returnPorudzbine);
        }

        // GET: api/Porudzbina/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Porudzbina>> GetPorudzbina(int id)
        {
            Models_Backend.Porudzbina returnPorudzbina = new Models_Backend.Porudzbina();

            Models.Porudzbina porudzbina = await _context.Porudzbina.Include(x => x.IdpotrosacNavigation).
                Include(x => x.IddostavljacNavigation).Include(x => x.Sadrzi).ThenInclude(x => x.IdproizvodNavigation).
                FirstOrDefaultAsync(x=>x.IdPorudzbine == id);

            returnPorudzbina = conversions.ConversionPorudzbina(porudzbina);

            if (returnPorudzbina == null)
            {
                return NotFound();
            }

            return Ok(returnPorudzbina);
        }

        // PUT: api/Porudzbina/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPorudzbina(int id, Porudzbina porudzbina)
        {
            if (id != porudzbina.IdPorudzbine)
            {
                return BadRequest();
            }

            _context.Entry(porudzbina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PorudzbinaExists(id))
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

        // POST: api/Porudzbina
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Porudzbina>> PostPorudzbina(object porudzbinaJson)
        {
            Models_Backend.Porudzbina porudzbinaP = JsonConvert.DeserializeObject<Models_Backend.Porudzbina>(porudzbinaJson.ToString());

            Potrosac p = _context.Potrosac.Where(x => x.IdPotrosac == porudzbinaP.Potrosac).FirstOrDefault();


            Porudzbina porudzbina = new Porudzbina()
            {
                AdresaDostave = porudzbinaP.AdresaDostave,
                Komentar = porudzbinaP.Komentar,
                StatusPorudzbine = porudzbinaP.StatusPorudzbine,
                Cijena = porudzbinaP.Cijena,
                Idpotrosac = porudzbinaP.Potrosac,
                IdpotrosacNavigation = p
            };

            _context.Porudzbina.Add(porudzbina);
            await _context.SaveChangesAsync();


            foreach (var item in porudzbinaP.proizvodi)
            {
                Sadrzi sadrzi = new Sadrzi();

                sadrzi.Idproizvod = item.IdProizvod;
                sadrzi.Idprorudzbina = porudzbina.IdPorudzbine;
                sadrzi.KolicinaProizvoda = porudzbinaP.KolicinaProizvoda[item.IdProizvod];
             
                _context.Sadrzi.Add(sadrzi);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
           

            return CreatedAtAction("GetPorudzbina", JsonConvert.SerializeObject(porudzbina.IdPorudzbine));
        }


        [HttpPost("{id}")]
        public async Task<ActionResult<Porudzbina>> PostPorudzbinaPotrosac(object idPotrosac)
        {
            Structs.JsonPorudzbina jsonPotrosac = JsonConvert.DeserializeObject<Structs.JsonPorudzbina>(idPotrosac.ToString());
            List<Models_Backend.Porudzbina> returnPorudzbine = new List<Models_Backend.Porudzbina>();

            List<Models.Porudzbina> porudzbine = await _context.Porudzbina.Include(x => x.IdpotrosacNavigation).
                Include(x => x.IddostavljacNavigation).Include(x => x.Sadrzi).ThenInclude(x => x.IdproizvodNavigation).
                Where(x => x.Idpotrosac == jsonPotrosac.IdPotrosac).Where(x=>x.StatusPorudzbine == 2).ToListAsync();

            porudzbine.ForEach(x => returnPorudzbine.Add((conversions.ConversionPorudzbina(x))));

            return Ok(returnPorudzbine);
        }



        [HttpPost]
        [Route("ProveraPorudzbine")]
        public async Task<ActionResult<Porudzbina>> ProveraPorudzbine(object idPorudzbina)
        {
            Structs.JsonZavrseno jsonPorudzbina = JsonConvert.DeserializeObject<Structs.JsonZavrseno>(idPorudzbina.ToString());

            Porudzbina porudzbina = _context.Porudzbina.Where(x => x.IdPorudzbine == jsonPorudzbina.IdPorudzbina).FirstOrDefault();

            if(porudzbina.StatusPorudzbine == 1)
            {
                return Ok(new Structs.StatusResponse(1));
            }
            else
            {
                return Ok(new Structs.StatusResponse(0));
            }
        }




        // DELETE: api/Porudzbina/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Porudzbina>> DeletePorudzbina(int id)
        {
            var porudzbina = await _context.Porudzbina.FindAsync(id);
            if (porudzbina == null)
            {
                return NotFound();
            }

            _context.Porudzbina.Remove(porudzbina);
            await _context.SaveChangesAsync();

            return porudzbina;
        }

        private bool PorudzbinaExists(int id)
        {
            return _context.Porudzbina.Any(e => e.IdPorudzbine == id);
        }
    }
}
