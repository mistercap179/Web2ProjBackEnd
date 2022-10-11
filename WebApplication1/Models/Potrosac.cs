using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class Potrosac
    {
        public Potrosac()
        {
            Porudzbina = new HashSet<Porudzbina>();
        }

        public int IdPotrosac { get; set; }

        public virtual Osoba IdPotrosacNavigation { get; set; }
        public virtual ICollection<Porudzbina> Porudzbina { get; set; }
    }
}
