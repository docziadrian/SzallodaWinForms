using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public abstract class Szallas
    {
        public string Nev { get; set; }
        public string Cim { get; set; }
        public int Kapacitas { get; set; }
        public decimal AlapAr { get; set; }
        public bool Foglalt { get; protected set; }

        protected Szallas(string nev, string cim, int kapacitas, decimal alapAr)
        {
            Nev = nev;
            Cim = cim;
            Kapacitas = kapacitas;
            AlapAr = alapAr;
            Foglalt = false;
        }

        public abstract decimal SzamolVegosszeg(int ejszakakSzama);

        public virtual string LeirasKeszites()
        {
            return $"{Nev} - {Cim} (Kapacitás: {Kapacitas} fő)";
        }

        public virtual void Foglalas()
        {
            Foglalt = true;
        }

        public virtual void LemondFoglalas()
        {
            Foglalt = false;
        }

        protected virtual decimal SzolgaltatasokAra()
        {
            return 0;
        }

        public abstract string GetTipus();

        public virtual string ToFileString()
        {
            return $"{GetTipus()}|{Nev}|{Cim}|{Kapacitas}|{AlapAr}|{Foglalt}";
        }
    }
}
