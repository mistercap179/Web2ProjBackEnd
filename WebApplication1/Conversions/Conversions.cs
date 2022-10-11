using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models_Backend;

namespace WebApplication1.Conversions
{
    public class Conversions : IConversions
    {
        public TipKorisnika tip;
        public Models_Backend.Dostavljac ConversionDostavljac(Models.Dostavljac dostavljac)
        {
            List<Models_Backend.Porudzbina> porudzbine = new List<Porudzbina>();
            dostavljac.Porudzbina.ToList().ForEach(x => porudzbine.Add(this.ConversionPorudzbina(x)));

            return new Models_Backend.Dostavljac(dostavljac.IdDostavljac, dostavljac.StatusProfila, porudzbine,
                dostavljac.IdDostavljacNavigation.Id, dostavljac.IdDostavljacNavigation.KorisnickoIme,
                dostavljac.IdDostavljacNavigation.Email, dostavljac.IdDostavljacNavigation.Lozinka,
                dostavljac.IdDostavljacNavigation.ImePrezime, dostavljac.IdDostavljacNavigation.DatumRodjenja,
                dostavljac.IdDostavljacNavigation.Adresa,
                dostavljac.IdDostavljacNavigation.Slika, TipKorisnika.Dostavljac);
        }

        public Models_Backend.Osoba ConversionOsoba(Models.Osoba osoba)
        {
           switch (osoba.TipKorisnika)
            {
                case 1:
                    tip = TipKorisnika.Admin;
                    break;
                case 2:
                    tip = TipKorisnika.Dostavljac;
                    break;
                case 3:
                    tip = TipKorisnika.Potrosac;
                    break;
           }
            return new Models_Backend.Osoba(osoba.Id, osoba.KorisnickoIme, osoba.Email, osoba.Lozinka, osoba.ImePrezime, osoba.DatumRodjenja, osoba.Adresa, osoba.Slika, tip);
        }

        public Porudzbina ConversionPorudzbina(Models.Porudzbina porudzbina)
        {
            List<Models_Backend.Proizvod> proizvodi = new List<Proizvod>();
            porudzbina.Sadrzi.ToList().ForEach(p=>proizvodi.Add(this.ConversionProizvod(p.IdproizvodNavigation)));

            Dictionary<int?, int> kolicineProizvoda = new Dictionary<int?, int>();
            // key idProizvod value kolicina

            foreach (var item in porudzbina.Sadrzi)
            {
                kolicineProizvoda.Add(item.Idproizvod, item.KolicinaProizvoda);
            }

            return new Models_Backend.Porudzbina(porudzbina.IdPorudzbine, porudzbina.AdresaDostave,
                  porudzbina.Komentar, porudzbina.Cijena, porudzbina.StatusPorudzbine,
                  porudzbina.Idpotrosac,
                  porudzbina.Iddostavljac,
                  kolicineProizvoda, proizvodi);
        }

        public Models_Backend.Potrosac ConversionPotrosac(Models.Potrosac potrosac)
        {
            List<Models_Backend.Porudzbina> porudzbine = new List<Porudzbina>();
            potrosac.Porudzbina.ToList().ForEach(x => porudzbine.Add(this.ConversionPorudzbina(x)));

            return new Models_Backend.Potrosac(potrosac.IdPotrosac, porudzbine, potrosac.IdPotrosacNavigation.Id, 
                potrosac.IdPotrosacNavigation.KorisnickoIme, potrosac.IdPotrosacNavigation.Email,
                potrosac.IdPotrosacNavigation.Lozinka, potrosac.IdPotrosacNavigation.ImePrezime, 
                potrosac.IdPotrosacNavigation.DatumRodjenja, potrosac.IdPotrosacNavigation.Adresa, 
                potrosac.IdPotrosacNavigation.Slika,TipKorisnika.Potrosac);
        }

        public Models_Backend.Proizvod ConversionProizvod(Models.Proizvod proizvod)
        {
            return new Models_Backend.Proizvod(proizvod.IdProizvod, proizvod.Ime, proizvod.Cijena, proizvod.Sastojci);
        }
    }
}
