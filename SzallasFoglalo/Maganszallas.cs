using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public abstract class Maganszallas : Szallas
    {
        public string TulajdonosNeve { get; set; }
        public string Telefonszam { get; set; }

        protected Maganszallas(string nev, string cim, int kapacitas, decimal alapAr,
            string tulajdonosNeve, string telefonszam)
            : base(nev, cim, kapacitas, alapAr)
        {
            TulajdonosNeve = tulajdonosNeve;
            Telefonszam = telefonszam;
        }

        public override string LeirasKeszites()
        {
            return base.LeirasKeszites() + $" | Tulajdonos: {TulajdonosNeve} ({Telefonszam})";
        }

        public override string ToFileString()
        {
            return base.ToFileString() + $"|{TulajdonosNeve}|{Telefonszam}";
        }
    }
}
