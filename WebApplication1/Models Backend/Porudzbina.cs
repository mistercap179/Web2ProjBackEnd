using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models_Backend
{
    public class Porudzbina
    {
        public int IdPorudzbine { get; set; }
        public string AdresaDostave { get; set; }
        public string Komentar { get; set; }
        public double Cijena { get; set; }
        public int StatusPorudzbine { get; set; }
        public int Potrosac { get; set; }
        public int? Dostavljac { get; set; }
        public Dictionary<int?, int> KolicinaProizvoda { get; set; }
        public List<Proizvod> proizvodi { get; set; }

        public Porudzbina(int id,string adresa,string komentar,
            double cijena,int status,int potrosac,int? dostavljac, Dictionary<int?, int> kolicinaP,List<Proizvod> proizvods)
        {
            this.IdPorudzbine = id;
            this.AdresaDostave = adresa;
            this.Komentar = komentar;
            this.Cijena = cijena;
            this.StatusPorudzbine = status;
            this.Potrosac = potrosac;
            this.Dostavljac = dostavljac;
            this.KolicinaProizvoda = kolicinaP;
            this.proizvodi = proizvods;
        }

        public Porudzbina() { }
    }
}
