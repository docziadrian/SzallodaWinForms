using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SzallasFoglalo
{
    public static class FileIO
    {
        private const string SzallasokFajl = "szallasok.txt";
        private const string FoglalasokFajl = "foglalasok.txt";

        public static void MentSzallasok(List<Szallas> szallasok)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SzallasokFajl, false, Encoding.UTF8))
                {
                    foreach (var szallas in szallasok)
                    {
                        writer.WriteLine(szallas.ToFileString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a szállások mentése közben: {ex.Message}");
            }
        }

        public static List<Szallas> BetoltSzallasok()
        {
            List<Szallas> szallasok = new List<Szallas>();

            if (!File.Exists(SzallasokFajl))
                return szallasok;

            try
            {
                using (StreamReader reader = new StreamReader(SzallasokFajl, Encoding.UTF8))
                {
                    string sor;
                    while ((sor = reader.ReadLine()) != null)
                    {
                        var szallas = ParseSzallas(sor);
                        if (szallas != null)
                            szallasok.Add(szallas);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a szállások betöltése közben: {ex.Message}");
            }

            return szallasok;
        }

        private static Szallas ParseSzallas(string sor)
        {
            var reszek = sor.Split('|');
            if (reszek.Length < 6) return null;

            string tipus = reszek[0];
            string nev = reszek[1];
            string cim = reszek[2];
            int kapacitas = int.Parse(reszek[3]);
            decimal alapAr = decimal.Parse(reszek[4]);
            bool foglalt = bool.Parse(reszek[5]);

            Szallas szallas = null;

            switch (tipus)
            {
                case "Hotel":
                    if (reszek.Length >= 10)
                    {
                        bool vanEtterem = bool.Parse(reszek[6]);
                        bool vanMediaszoba = bool.Parse(reszek[7]);
                        int csillagok = int.Parse(reszek[8]);
                        bool vanSzallodai = bool.Parse(reszek[9]);
                        szallas = new Hotel(nev, cim, kapacitas, alapAr, vanEtterem, vanMediaszoba, csillagok, vanSzallodai);
                    }
                    break;

                case "Panzio":
                    if (reszek.Length >= 9)
                    {
                        bool vanEtterem = bool.Parse(reszek[6]);
                        bool vanMediaszoba = bool.Parse(reszek[7]);
                        bool vanReggeli = bool.Parse(reszek[8]);
                        szallas = new Panzio(nev, cim, kapacitas, alapAr, vanEtterem, vanMediaszoba, vanReggeli);
                    }
                    break;

                case "Vendeghaz":
                    if (reszek.Length >= 9)
                    {
                        string tulajdonos = reszek[6];
                        string telefon = reszek[7];
                        bool vanKonyha = bool.Parse(reszek[8]);
                        szallas = new Vendeghaz(nev, cim, kapacitas, alapAr, tulajdonos, telefon, vanKonyha);
                    }
                    break;

                case "Apartman":
                    if (reszek.Length >= 10)
                    {
                        string tulajdonos = reszek[6];
                        string telefon = reszek[7];
                        int szobak = int.Parse(reszek[8]);
                        bool vanErkely = bool.Parse(reszek[9]);
                        szallas = new Apartman(nev, cim, kapacitas, alapAr, tulajdonos, telefon, szobak, vanErkely);
                    }
                    break;
            }

            if (szallas != null && foglalt)
            {
                szallas.Foglalas();
            }

            return szallas;
        }

        public static void MentFoglalasok(List<Foglalas> foglalasok)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FoglalasokFajl, false, Encoding.UTF8))
                {
                    foreach (var foglalas in foglalasok)
                    {
                        writer.WriteLine(foglalas.ToFileString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a foglalások mentése közben: {ex.Message}");
            }
        }

        public static List<Foglalas> BetoltFoglalasok(List<Szallas> szallasok)
        {
            List<Foglalas> foglalasok = new List<Foglalas>();

            if (!File.Exists(FoglalasokFajl))
                return foglalasok;

            try
            {
                using (StreamReader reader = new StreamReader(FoglalasokFajl, Encoding.UTF8))
                {
                    string sor;
                    while ((sor = reader.ReadLine()) != null)
                    {
                        var reszek = sor.Split('|');
                        if (reszek.Length >= 5)
                        {
                            string vendegNeve = reszek[0];
                            string szallasNev = reszek[1];
                            DateTime erkezes = DateTime.Parse(reszek[2]);
                            int ejszakak = int.Parse(reszek[3]);

                            var szallas = szallasok.FirstOrDefault(s => s.Nev == szallasNev);
                            if (szallas != null)
                            {
                                var foglalas = new Foglalas(szallas, vendegNeve, erkezes, ejszakak);
                                foglalasok.Add(foglalas);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Hiba a foglalások betöltése közben: {ex.Message}");
            }

            return foglalasok;
        }
    }
}
