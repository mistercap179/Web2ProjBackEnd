using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models_Backend
{
    public class Osoba
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string ImePrezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public string Adresa { get; set; }
        public string Slika { get; set; }
        public TipKorisnika TipKorisnika { get; set; }

        public Osoba(int id,string kIme,string email,string lozinka,string ime,DateTime datum,string adresa,string slika,TipKorisnika tip)
        {
            this.Id = id;
            this.KorisnickoIme = kIme;
            this.Email = email;
            this.Lozinka = lozinka;
            this.ImePrezime = ime;
            this.DatumRodjenja = datum;
            this.Adresa = adresa;
            this.Slika = slika;
            this.TipKorisnika = tip;
        }

        public Osoba() { }
    }
}
