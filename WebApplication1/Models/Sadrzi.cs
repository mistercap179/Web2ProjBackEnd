using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Sadrzi
    {
        public int Idprorudzbina { get; set; }
        public int? Idproizvod { get; set; }
        public int IdSadrzi { get; set; }
        public int KolicinaProizvoda { get; set; }

        public virtual Proizvod IdproizvodNavigation { get; set; }
        public virtual Porudzbina IdprorudzbinaNavigation { get; set; }
    }
}
