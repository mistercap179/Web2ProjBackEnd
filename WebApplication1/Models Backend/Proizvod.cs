using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models_Backend
{
    public class Proizvod
    {
        public int IdProizvod { get; set; }
        public string Ime { get; set; }
        public double Cijena { get; set; }
        public string Sastojci { get; set; }

        public Proizvod(int id,string ime,double cijena,string sastojci)
        {
            this.IdProizvod = id;
            this.Ime = ime;
            this.Cijena = cijena;
            this.Sastojci = sastojci;
        }
    }
}
