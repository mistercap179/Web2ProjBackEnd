using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Dostavljac
    {
        public Dostavljac()
        {
            Porudzbina = new HashSet<Porudzbina>();
        }

        public int IdDostavljac { get; set; }
        public int StatusProfila { get; set; }

        public virtual Osoba IdDostavljacNavigation { get; set; }
        public virtual ICollection<Porudzbina> Porudzbina { get; set; }
    }
}
