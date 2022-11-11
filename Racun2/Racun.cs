using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Racun2
{
    public class Racun
    {
        public int RacunID { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public DateTime DatumIzmene { get; set; }
        public string NazivStatusa { get; set; }

        public List <Stavka> Stavke{ get; set; }

        // public float UkupnaCena => Stavke.Sum(p => p.Ukupno);

        public float UkupnaCena { get; set; }


        public void IspisRacuna()
        {
            
            System.Windows.Forms.MessageBox.Show($"{RacunID}  {DatumKreiranja}  {DatumIzmene}  {UkupnaCena}  {NazivStatusa}   ");
        }

        public void IzracunajUkupnuCenu()
        {
            UkupnaCena = Stavke.Sum(p => p.Ukupno);
        }


    }
}
