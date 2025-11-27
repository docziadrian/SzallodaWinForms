using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public class Apartman : Maganszallas
    {
        public int SzobakSzama { get; set; }
        public bool VanErkely { get; set; }

        public Apartman(string nev, string cim, int kapacitas, decimal alapAr,
            string tulajdonosNeve, string telefonszam, int szobakSzama, bool vanErkely)
            : base(nev, cim, kapacitas, alapAr, tulajdonosNeve, telefonszam)
        {
            SzobakSzama = szobakSzama;
            VanErkely = vanErkely;
        }

        public override decimal SzamolVegosszeg(int ejszakakSzama)
        {
            decimal alaposszeg = AlapAr * ejszakakSzama;
            decimal szorzo = 1 + (SzobakSzama * 0.05m);
            if (VanErkely) szorzo += 0.1m;
            return alaposszeg * szorzo;
        }

        public override string LeirasKeszites()
        {
            string leiras = base.LeirasKeszites();
            leiras += $" | {SzobakSzama} szobás";
            if (VanErkely) leiras += " (Erkélyes)";
            return leiras;
        }

        public override string GetTipus()
        {
            return "Apartman";
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{SzobakSzama}|{VanErkely}";
        }
    }
}
