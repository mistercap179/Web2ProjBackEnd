using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Conversions
{
    public interface IConversions
    {
        Models_Backend.Osoba ConversionOsoba(Models.Osoba osoba);
        Models_Backend.Dostavljac ConversionDostavljac(Models.Dostavljac dostavljac);
        Models_Backend.Potrosac ConversionPotrosac(Models.Potrosac potrosac);
        Models_Backend.Porudzbina ConversionPorudzbina(Models.Porudzbina porudzbina);
        Models_Backend.Proizvod ConversionProizvod(Models.Proizvod proizvod);
    }
}
