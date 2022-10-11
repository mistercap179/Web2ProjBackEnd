using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models_Backend
{
    public class Dostavljac : Osoba
    {
        public int IdDostavljac { get; set; }
        public int StatusProfila { get; set; }
        public List<Porudzbina> Porudzbine { get; set; }

        public Dostavljac(int id,int status,List<Porudzbina> p, int idO, string kIme, string email, string lozinka, string ime, DateTime datum, string adresa, string slika, TipKorisnika tip) :
            base(idO, kIme, email, lozinka, ime, datum, adresa, slika, tip)
        {
            this.IdDostavljac = id;
            this.StatusProfila = status;
            this.Porudzbine = p;
        }
    }
}
