using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models_Backend
{
    public class Potrosac : Osoba
    {
        public int IdPotrosac { get; set; }
        public List<Porudzbina> Porudzbine { get; set; }

        public Potrosac(int id,List<Porudzbina>p, int idO, string kIme, string email, string lozinka, string ime, DateTime datum, string adresa, string slika, TipKorisnika tip) :
            base(idO, kIme, email, lozinka, ime, datum, adresa, slika, tip)
        {
            this.IdPotrosac = id;
            this.Porudzbine = p;
        }

        public Potrosac() { }
    }
}
