using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Porudzbina
    {
        public Porudzbina()
        {
            Sadrzi = new HashSet<Sadrzi>();
        }

        public int IdPorudzbine { get; set; }
        public string AdresaDostave { get; set; }
        public string Komentar { get; set; }
        public double Cijena { get; set; }
        public int StatusPorudzbine { get; set; }
        public int Idpotrosac { get; set; }
        public int? Iddostavljac { get; set; }

        public virtual Dostavljac IddostavljacNavigation { get; set; }
        public virtual Potrosac IdpotrosacNavigation { get; set; }
        public virtual ICollection<Sadrzi> Sadrzi { get; set; }
    }
}
