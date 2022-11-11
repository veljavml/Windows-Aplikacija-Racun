using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racun2
{
    public class Stavka
    {
        public int StavkaID { get; set; }
        public int RacunId { get; set; }

        public int ProizvodID { get; set; }
        public float Kolicina { get; set; }

        public float Ukupno { get; set; }

        Proizvod p = new Proizvod();

        public void IzraunajUkupno()
        {
            Ukupno = p.Cena * Kolicina;
        }


        public string ispis()
        {
            return ($"Stavka: {StavkaID} RacunID: {RacunId}  ProizvodID: {ProizvodID}  Kolicina: {Kolicina}  Ukupno: {Ukupno}  Proizvod:{p.NazivProizvoda}");
        }
    }

}
