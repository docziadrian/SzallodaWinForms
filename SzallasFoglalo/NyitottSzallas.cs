using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public abstract class Nyitottszallas : Szallas
    {
        public bool VanEtterem { get; set; }
        public bool VanMediaszoba { get; set; }

        protected Nyitottszallas(string nev, string cim, int kapacitas, decimal alapAr,
            bool vanEtterem, bool vanMediaszoba)
            : base(nev, cim, kapacitas, alapAr)
        {
            VanEtterem = vanEtterem;
            VanMediaszoba = vanMediaszoba;
        }

        protected override decimal SzolgaltatasokAra()
        {
            decimal ar = 0;
            if (VanEtterem) ar += 5000;
            if (VanMediaszoba) ar += 3000;
            return ar;
        }

        public override string LeirasKeszites()
        {
            string leiras = base.LeirasKeszites();
            List<string> szolgaltatasok = new List<string>();
            if (VanEtterem) szolgaltatasok.Add("Étterem");
            if (VanMediaszoba) szolgaltatasok.Add("Médiaszoba");
            if (szolgaltatasok.Count > 0)
                leiras += $" [Szolgáltatások: {string.Join(", ", szolgaltatasok)}]";
            return leiras;
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{VanEtterem}|{VanMediaszoba}";
        }
    }
}
