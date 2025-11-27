using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public class Hotel : Nyitottszallas
    {
        public int CsillagokSzama { get; set; }
        public bool VanSzallodaiSzolgaltatas { get; set; }

        public Hotel(string nev, string cim, int kapacitas, decimal alapAr,
            bool vanEtterem, bool vanMediaszoba, int csillagokSzama, bool vanSzallodaiSzolgaltatas)
            : base(nev, cim, kapacitas, alapAr, vanEtterem, vanMediaszoba)
        {
            CsillagokSzama = csillagokSzama;
            VanSzallodaiSzolgaltatas = vanSzallodaiSzolgaltatas;
        }

        public override decimal SzamolVegosszeg(int ejszakakSzama)
        {
            decimal alaposszeg = AlapAr * ejszakakSzama;
            decimal szorzo = 1 + (CsillagokSzama * 0.1m);
            decimal szolgaltatasok = SzolgaltatasokAra();
            if (VanSzallodaiSzolgaltatas) szolgaltatasok += 8000;
            return (alaposszeg * szorzo) + szolgaltatasok;
        }

        public override string LeirasKeszites()
        {
            string leiras = base.LeirasKeszites();
            leiras += $" | {CsillagokSzama} csillagos hotel";
            if (VanSzallodaiSzolgaltatas) leiras += " (Prémium szolgáltatás)";
            return leiras;
        }

        public override string GetTipus()
        {
            return "Hotel";
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{CsillagokSzama}|{VanSzallodaiSzolgaltatas}";
        }
    }
}
