using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public class Panzio : Nyitottszallas
    {
        public bool VanReggeli { get; set; }

        public Panzio(string nev, string cim, int kapacitas, decimal alapAr,
            bool vanEtterem, bool vanMediaszoba, bool vanReggeli)
            : base(nev, cim, kapacitas, alapAr, vanEtterem, vanMediaszoba)
        {
            VanReggeli = vanReggeli;
        }

        public override decimal SzamolVegosszeg(int ejszakakSzama)
        {
            decimal alaposszeg = AlapAr * ejszakakSzama;
            decimal szolgaltatasok = SzolgaltatasokAra();
            if (VanReggeli) szolgaltatasok += 2500 * ejszakakSzama;
            return alaposszeg + szolgaltatasok;
        }

        public override string LeirasKeszites()
        {
            string leiras = base.LeirasKeszites();
            if (VanReggeli) leiras += " (Reggelivel)";
            return leiras;
        }

        public override string GetTipus()
        {
            return "Panzio";
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{VanReggeli}";
        }
    }
}
