using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Proizvod
    {
        public Proizvod()
        {
            Sadrzi = new HashSet<Sadrzi>();
        }
        public int IdProizvod { get; set; }
        public string Ime { get; set; }
        public double Cijena { get; set; }
        public string Sastojci { get; set; }

        public virtual ICollection<Sadrzi> Sadrzi { get; set; }
    }
}
