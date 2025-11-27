using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public class Foglalas
    {
        public Szallas Szallas { get; set; }
        public string VendegNeve { get; set; }
        public DateTime ErkezesiDatum { get; set; }
        public int EjszakakSzama { get; set; }
        public decimal Vegosszeg { get; set; }

        public Foglalas(Szallas szallas, string vendegNeve, DateTime erkezesiDatum, int ejszakakSzama)
        {
            Szallas = szallas;
            VendegNeve = vendegNeve;
            ErkezesiDatum = erkezesiDatum;
            EjszakakSzama = ejszakakSzama;
            Vegosszeg = szallas.SzamolVegosszeg(ejszakakSzama);
        }

        public string ToFileString()
        {
            return $"{VendegNeve}|{Szallas.Nev}|{ErkezesiDatum:yyyy-MM-dd}|{EjszakakSzama}|{Vegosszeg}";
        }
    }
}
