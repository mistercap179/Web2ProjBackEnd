using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using static WebApplication1.Models.Structs;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using EmailService;
using WebApplication1.Conversions;

namespace WebApplication1.Controllers
{

    [Route("api/{controller}")]
    [ApiController]
    public class OsobaController : ControllerBase
    {
        private readonly IConversions conversions ;
        private readonly ApplicationSettings _appSettings;
        private readonly DostavaContext _context;
        //private readonly IEmailSender _emailSender;
        public OsobaController(DostavaContext context, IOptions<ApplicationSettings> appSettings,IConversions _conversions)
        {
            _context = context;
            _appSettings = appSettings.Value;
            conversions = _conversions;
        }

        // GET: api/Osoba
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Osoba>>> GetOsoba()
        {
            List<Models_Backend.Osoba> osobe = new List<Models_Backend.Osoba>();
            await _context.Osoba.ForEachAsync(x => osobe.Add(conversions.ConversionOsoba(x)));

            return Ok(osobe);
        }

        // GET: api/Osoba/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Osoba>> GetOsoba(int id)
        {
            Models_Backend.Osoba osoba = conversions.ConversionOsoba(await _context.Osoba.FindAsync(id));

            if (osoba == null)
            {
                return NotFound();
            }

            return Ok(osoba);
        }

        

        // PUT: api/Osoba/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOsoba(int id,[FromBody] object osoba)
        {
            Osoba osobab = JsonConvert.DeserializeObject<Osoba>(osoba.ToString());

            Osoba osobaBaza = _context.Osoba.Where(x => x.Id == osobab.Id).FirstOrDefault();
           

            string slika = "assets/" + osobab.Slika.Split("fakepath\\")[1];

            osobab.Slika = slika;

            osobaBaza.KorisnickoIme = osobab.KorisnickoIme;
            osobaBaza.Lozinka = osobab.Lozinka;
            osobaBaza.Adresa = osobab.Adresa;
            osobaBaza.Slika = osobab.Slika;
            osobaBaza.Email = osobab.Email;



            if (id != osobaBaza.Id)
            {
                return BadRequest();
            }

            _context.Entry(osobaBaza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OsobaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(JsonConvert.SerializeObject(conversions.ConversionOsoba(osobaBaza)));
        }

        // POST: api/Osoba
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[EnableCors("AllowOrigin")]
        [HttpPost]
        public async Task<ActionResult<Osoba>> PostOsoba(object osoba)
        {

            Structs.JsonReturnUserExist jsonReturnUser;
            Osoba osobab = JsonConvert.DeserializeObject<Osoba>(osoba.ToString());

            string slika = "assets/" + osobab.Slika.Split("fakepath\\")[1];

            osobab.Slika = slika;

            if (true == UserNameExists(osobab.KorisnickoIme) && true == EmailExists(osobab.Email))
            {
                jsonReturnUser.email = "found";
                jsonReturnUser.userName = "found";
                return CreatedAtAction("GetOsoba", JsonConvert.SerializeObject(jsonReturnUser));
            }
            else if(true == UserNameExists(osobab.KorisnickoIme) || true == EmailExists(osobab.Email))
            {
                jsonReturnUser.email = "found";
                jsonReturnUser.userName = "found";
                return CreatedAtAction("GetOsoba", JsonConvert.SerializeObject(jsonReturnUser));
            }
            else
            {

                if(osobab.TipKorisnika == 1)
                {
                    _context.Osoba.Add(osobab);
                    await _context.SaveChangesAsync();

                }
                else if (osobab.TipKorisnika == 2)
                {
                    Dostavljac dostavljac = new Dostavljac() { IdDostavljac = osobab.Id, StatusProfila = 2 };
                    osobab.Dostavljac = dostavljac;
                    _context.Osoba.Add(osobab);
                    await _context.SaveChangesAsync();
                    _context.Dostavljac.Add(dostavljac);
                }

                else if(osobab.TipKorisnika == 3)
                {
                    Potrosac potrosac = new Potrosac() { IdPotrosac = osobab.Id }; 
                    osobab.Potrosac = potrosac;
                    _context.Osoba.Add(osobab);
                    await _context.SaveChangesAsync();
                    _context.Potrosac.Add(potrosac);

                }
                return CreatedAtAction("GetOsoba", osobab);
            }
        }


        [HttpPost]
        [Route("DodajSliku")]
        public async Task<ActionResult<String>> DodajSliku([FromForm] IFormCollection slika)
        {
            if (slika.Files.Count == 0)
            {
                return BadRequest();
            }

            var f = slika.Files[0];
            var path = Path.Combine(Directory.GetCurrentDirectory(), "slike", f.FileName);

            using (Stream stream = f.OpenReadStream())
            {
                using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    while (stream.Position < stream.Length)
                    {
                        fileStream.WriteByte((byte)stream.ReadByte());
                    }
                }
            }

            return Ok(JsonConvert.SerializeObject(path));
        }






        [HttpPost]
        [Route("Login")]
        //POST : /api/Osoba/Login
        public async Task<IActionResult> Login(object model)
        {
            LoginModel loginUser = JsonConvert.DeserializeObject<LoginModel>(model.ToString());
            if (await _context.Osoba.AnyAsync(x=>x.Email == loginUser.Email 
                                              && x.Lozinka == loginUser.Lozinka)) {

                Osoba osoba = _context.Osoba.Where(x => x.Email == loginUser.Email).FirstOrDefault();
                
                string role;

                if (osoba.TipKorisnika == 1)
                {
                    role = "Admin";
                }
                else if (osoba.TipKorisnika == 2)
                {
                    role = "Dost";
                }
                else
                {
                    role = "Potrosac";
                }


                if (role == "Dost" && _context.Dostavljac.Where(x => x.IdDostavljac == osoba.Id).FirstOrDefault().StatusProfila == 0) 
                {
                    JsonResponseLoginFail jsonResponseLoginFail = new JsonResponseLoginFail() { message = "Vas profil je blokiran!" };
                    return Ok(JsonConvert.SerializeObject(jsonResponseLoginFail));
                }

                else if (role == "Dost" && _context.Dostavljac.Where(x => x.IdDostavljac == osoba.Id).FirstOrDefault().StatusProfila == 2)
                {
                    JsonResponseLoginFail jsonResponseLoginFail = new JsonResponseLoginFail() { message = "Vas profil jos uvijek nije prihvacen od strane administratora!" };
                    return Ok(JsonConvert.SerializeObject(jsonResponseLoginFail));
                }

                else
                {
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                       {
                        new Claim(ClaimTypes.Role, role)
                       }),
                        Expires = DateTime.UtcNow.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = "https://localhost:5001",
                        Audience = "https://localhost:44327/api/"
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var token = tokenHandler.WriteToken(securityToken);
                    osoba.Dostavljac = null;
                    JsonResponseLogin jsonResponse = new JsonResponseLogin { role = role, token = token, osoba = osoba };
                    try
                    {
                        return Ok(JsonConvert.SerializeObject(jsonResponse));
                    }
                    catch(Exception e)
                    {
                        return Ok(JsonConvert.SerializeObject(jsonResponse));
                    }
                    

                }

            }
            else
            {
                JsonResponsePasswordFail jsonResponsePassword = new JsonResponsePasswordFail() { poruka = "Pogresan email ili sifra!" };
                return Ok(JsonConvert.SerializeObject(jsonResponsePassword));
            }

        }


        // DELETE: api/Osoba/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Osoba>> DeleteOsoba(int id)
        {
            var osoba = await _context.Osoba.FindAsync(id);
            if (osoba == null)
            {
                return NotFound();
            }

            _context.Osoba.Remove(osoba);
            await _context.SaveChangesAsync();

            return osoba;
        }

        private bool OsobaExists(int id)
        {
            return _context.Osoba.Any(e => e.Id == id);
        }

        private bool UserNameExists(string userName)
        {
            return _context.Osoba.Any(e => e.KorisnickoIme == userName);
        }

        private bool EmailExists(string email)
        {
            return _context.Osoba.Any(e => e.Email == email);
        }

        private bool PasswordExists(string password)
        {
            return _context.Osoba.Any(e => e.Lozinka == password);
        }

    }
}
