using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public class Vendeghaz : Maganszallas
    {
        public bool VanKozosKonyha { get; set; }

        public Vendeghaz(string nev, string cim, int kapacitas, decimal alapAr,
            string tulajdonosNeve, string telefonszam, bool vanKozosKonyha)
            : base(nev, cim, kapacitas, alapAr, tulajdonosNeve, telefonszam)
        {
            VanKozosKonyha = vanKozosKonyha;
        }

        public override decimal SzamolVegosszeg(int ejszakakSzama)
        {
            decimal alaposszeg = AlapAr * ejszakakSzama;
            if (VanKozosKonyha) alaposszeg *= 0.95m;
            return alaposszeg;
        }

        public override string LeirasKeszites()
        {
            string leiras = base.LeirasKeszites();
            if (VanKozosKonyha) leiras += " (Közös konyha)";
            return leiras;
        }

        public override string GetTipus()
        {
            return "Vendeghaz";
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{VanKozosKonyha}";
        }
    }
}
