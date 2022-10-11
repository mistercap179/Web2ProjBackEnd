using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;
using static WebApplication1.Models.Structs;
using WebApplication1.Conversions;
using Microsoft.Extensions.Options;

namespace WebApplication1.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class DostavljacController : ControllerBase
    {
        private readonly IConversions conversions;
        private readonly DostavaContext _context;
        private readonly ApplicationSettings _appSettings;
        public DostavljacController(DostavaContext context, IConversions _conversions, IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
            conversions = _conversions;
            _appSettings = appSettings.Value;
        }

        // GET: api/Dostavljac
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dostavljac>>> GetDostavljac()
        {
            List<Models_Backend.Dostavljac> dostavljaci = new List<Models_Backend.Dostavljac>();
            
            await _context.Dostavljac.Include(x=>x.Porudzbina).Include(x=>x.IdDostavljacNavigation).
                ForEachAsync(x=>dostavljaci.Add(conversions.ConversionDostavljac(x)));

            return Ok(JsonConvert.SerializeObject(dostavljaci));
        }

        // GET: api/Dostavljac/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dostavljac>> GetDostavljac(int id)
        {
            Models_Backend.Dostavljac dostavljac = conversions.ConversionDostavljac(await _context.Dostavljac.
                Include(x => x.Porudzbina).
                Include(x=>x.IdDostavljacNavigation).FirstOrDefaultAsync(x=>x.IdDostavljac == id));

            if (dostavljac == null)
            {
                return NotFound();
            }

            return Ok(dostavljac);
        }

        // PUT: api/Dostavljac/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDostavljac(int id, Dostavljac dostavljac)
        {
            if (id != dostavljac.IdDostavljac)
            {
                return BadRequest();
            }

            _context.Entry(dostavljac).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DostavljacExists(id))
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

        // POST: api/Dostavljac
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Dostavljac>> PostDostavljac(Dostavljac dostavljac)
        {
            _context.Dostavljac.Add(dostavljac);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DostavljacExists(dostavljac.IdDostavljac))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDostavljac", new { id = dostavljac.IdDostavljac }, dostavljac);
        }



        [HttpPost]
        [Route("PostPrihvatiDostavu")]
        public async Task<ActionResult<Dostavljac>> PostPrihvatiDostavu(object dostava)
        {
            Structs.JsonDostava jsonDostava = JsonConvert.DeserializeObject<Structs.JsonDostava>(dostava.ToString());

            Porudzbina porudzbina = _context.Porudzbina.Where(x => x.IdPorudzbine == jsonDostava.IdPorudzbina).FirstOrDefault();

            porudzbina.StatusPorudzbine = 1;
            porudzbina.Iddostavljac = jsonDostava.dostavljac.IdDostavljac;
            //porudzbina.IddostavljacNavigation = jsonDostava.dostavljac;   /// mozda izostaviti

            lock (_context)
            { 
                _context.Entry(porudzbina).State = EntityState.Modified;
                _context.SaveChanges();
            }
           

            return NoContent();
        }



        [HttpPost]
        [Route("ZavrsenaPorudzbina")]
        public async Task<ActionResult<Dostavljac>> ZavrsenaPorudzbina(object zavrseno)
        {
            Structs.JsonZavrseno jsonZavrseno = JsonConvert.DeserializeObject<Structs.JsonZavrseno>(zavrseno.ToString());

            Porudzbina porudzbina = _context.Porudzbina.Where(x => x.IdPorudzbine == jsonZavrseno.IdPorudzbina).FirstOrDefault();

            porudzbina.StatusPorudzbine = 2;

            _context.Entry(porudzbina).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }




        [HttpPost]
        [Route("PostDostavljac")]
        public async Task<ActionResult<Dostavljac>> PostDostavljac(object idDostavljac)
        {
            Structs.JsonPorudzbinaDostavljac jsonPorudzbinaDostavljac = JsonConvert.DeserializeObject<Structs.JsonPorudzbinaDostavljac>(idDostavljac.ToString());
            List<Models_Backend.Porudzbina> returnPorudzbine = new List<Models_Backend.Porudzbina>();

            List<Models.Porudzbina> porudzbine = await _context.Porudzbina.Include(x => x.IdpotrosacNavigation).
                Include(x => x.IddostavljacNavigation).Include(x => x.Sadrzi).ThenInclude(x => x.IdproizvodNavigation).
                Where(x => x.Iddostavljac == jsonPorudzbinaDostavljac.IdDostavljac).Where(x => x.StatusPorudzbine == 2).ToListAsync();

            porudzbine.ForEach(x => returnPorudzbine.Add((conversions.ConversionPorudzbina(x))));

            return Ok(returnPorudzbine);
        }


        [HttpPost]
        [Route("GetNove")]
        public async Task<ActionResult<Dostavljac>> GetNove(object idDostavljac)
        {
            Structs.JsonPorudzbinaDostavljac jsonPorudzbinaDostavljac = JsonConvert.DeserializeObject<Structs.JsonPorudzbinaDostavljac>(idDostavljac.ToString());
            List<Models_Backend.Porudzbina> returnPorudzbine = new List<Models_Backend.Porudzbina>();

            List<Models.Porudzbina> porudzbine = await _context.Porudzbina.Include(x => x.IdpotrosacNavigation).
                Include(x => x.IddostavljacNavigation).Include(x => x.Sadrzi).ThenInclude(x => x.IdproizvodNavigation).
                Where(x => x.StatusPorudzbine == 0).ToListAsync();

            porudzbine.ForEach(x => returnPorudzbine.Add((conversions.ConversionPorudzbina(x))));

            return Ok(returnPorudzbine);
        }



        [HttpPost]
        [Route("GetUtoku")]
        public async Task<ActionResult<Dostavljac>> GetUtoku(object idDostavljac)
        {
            Structs.JsonPorudzbinaDostavljac jsonPorudzbinaDostavljac = JsonConvert.DeserializeObject<Structs.JsonPorudzbinaDostavljac>(idDostavljac.ToString());
            List<Models_Backend.Porudzbina> returnPorudzbine = new List<Models_Backend.Porudzbina>();

            List<Models.Porudzbina> porudzbine = await _context.Porudzbina.Include(x => x.IdpotrosacNavigation).
                Include(x => x.IddostavljacNavigation).Include(x => x.Sadrzi).ThenInclude(x => x.IdproizvodNavigation).
                Where(x => x.StatusPorudzbine == 1).Where(x=>x.Iddostavljac == jsonPorudzbinaDostavljac.IdDostavljac).ToListAsync();

            porudzbine.ForEach(x => returnPorudzbine.Add((conversions.ConversionPorudzbina(x))));

            return Ok(returnPorudzbine);
        }





        [HttpPost]
        [Route("ProveraStatus")]
        public async Task<ActionResult<Dostavljac>> ProveraStatus(object idDostavljac)
        {
            Structs.JsonPorudzbinaDostavljac jsonPorudzbinaDostavljac = JsonConvert.DeserializeObject<Structs.JsonPorudzbinaDostavljac>(idDostavljac.ToString());

            Dostavljac d =_context.Dostavljac.Where(x => x.IdDostavljac == jsonPorudzbinaDostavljac.IdDostavljac).FirstOrDefault();

            JsonBlokiran jsonBlokiran = new JsonBlokiran() { status = d.StatusProfila };
            return Ok(JsonConvert.SerializeObject(jsonBlokiran));
            
        }






        [HttpPost("{id}")]
        public async Task<ActionResult<Dostavljac>> ChangeStatusDostavljac(object dost)
        {
            Dostavljac dostavljac = JsonConvert.DeserializeObject<Dostavljac>(dost.ToString());

            _context.Entry(dostavljac).State = EntityState.Modified;


            MailMessage message = new MailMessage();

            if(dostavljac.StatusProfila == 0)
            {
                message.Subject = "Verifikacija";
                message.Body = "Profil blokiran";
                message.From = new MailAddress(_appSettings.Email, _appSettings.EmailSenderName, false);
            }
            else if(dostavljac.StatusProfila == 1)
            {
                message.Subject = "Verifikacija";
                message.Body = "Profil verifikovan";
                message.From = new MailAddress(_appSettings.Email, _appSettings.EmailSenderName, false);
            }
            else
            {
                message.Subject = "Verifikacija";
                message.Body = "Profil na cekanju";
                message.From = new MailAddress(_appSettings.Email, _appSettings.EmailSenderName, false);
            }

            // Set subject of the message, body and sender information
            

            // Add To recipients and CC recipients
            message.To.Add(new MailAddress("markovicf999@outlook.com", "Filip Markovic", false));

            // Save message in EML, EMLX, MSG and MHTML formats
            message.Save("EmailMessage.eml", SaveOptions.DefaultEml);
            message.Save("EmailMessage.emlx", SaveOptions.CreateSaveOptions(MailMessageSaveType.EmlxFormat));
            message.Save("EmailMessage.msg", SaveOptions.DefaultMsgUnicode);
            message.Save("EmailMessage.mhtml", SaveOptions.DefaultMhtml);
            SmtpClient client = new SmtpClient();

            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = _appSettings.EmailHost;
            client.Username = _appSettings.Email;
            client.Password = _appSettings.EmailPassword;
            client.Port = 587;
            client.SecurityOptions = SecurityOptions.SSLExplicit;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
            }



            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!DostavljacExists(dostavljac.IdDostavljac))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(JsonConvert.SerializeObject(dostavljac));
        }






        // DELETE: api/Dostavljac/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Dostavljac>> DeleteDostavljac(int id)
        {
            var dostavljac = await _context.Dostavljac.FindAsync(id);
            if (dostavljac == null)
            {
                return NotFound();
            }

            _context.Dostavljac.Remove(dostavljac);
            await _context.SaveChangesAsync();

            return dostavljac;
        }

        private bool DostavljacExists(int id)
        {
            return _context.Dostavljac.Any(e => e.IdDostavljac == id);
        }
    }
}
