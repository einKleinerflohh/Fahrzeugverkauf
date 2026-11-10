using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    class Haendler
    {
        public string haendlerName { get; set; }
        public int geduld { get; set; }
        public string aktuelleStimmung { get; private set; }
        private List<Fahrzeug> fahrzeugBestand = new List<Fahrzeug>();
        private Random rnd = new Random();

        private string[] stimmungen = { "Gut gelaunt", "Genervt", "Unberechenbar", "Streng", "Entspannt" };

        public Haendler() { }

        public Haendler(string name, int startGeduld)
        {
            haendlerName = name;
            geduld = startGeduld;
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
        }

        public void fahrzeugHinzufuegen(Fahrzeug f) => fahrzeugBestand.Add(f);
        public void fahrzeugEntfernen(Fahrzeug f) => fahrzeugBestand.Remove(f);
        public List<Fahrzeug> getBestand() => fahrzeugBestand;

        public int GeneriereMaxVersuche()
        {
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
            return rnd.Next(2, 6);
        }

        public VerhandlungsErgebnis VerarbeiteVerhandlung(bool istKauf, Fahrzeug f, int aktion, double input, ref double preis, ref int geduld)
        {
            if (istKauf)
            {
                double stimmungsFaktor = aktuelleStimmung == "Gut gelaunt" || aktuelleStimmung == "Entspannt" ? 0.04 :
                                         (aktuelleStimmung == "Genervt" || aktuelleStimmung == "Streng" ? -0.04 : (rnd.NextDouble() * 0.1 - 0.05));

                double sk = f.preis * (0.78 + rnd.NextDouble() * 0.08 + stimmungsFaktor);
                double fk = f.preis * (0.65 + rnd.NextDouble() * 0.06);

                if (aktion == 1)
                {
                    if (input < fk) return VerhandlungsErgebnis.Frechheit;
                    if (input >= preis) return VerhandlungsErgebnis.Deal;

                    geduld--;
                    double sprungFaktor = 0.3 + rnd.NextDouble() * 0.4;
                    preis = Math.Max(sk, Math.Round(preis - (preis - input) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Entgegenkommen;
                }
                if (aktion == 2)
                {
                    bool erfolg = rnd.NextDouble() < (aktuelleStimmung == "Gut gelaunt" ? 0.8 : (aktuelleStimmung == "Genervt" ? 0.3 : 0.55));
                    if (erfolg)
                    {
                        double rabatt = 0.02 + rnd.NextDouble() * 0.06;
                        preis = Math.Max(sk, Math.Round(preis - f.preis * rabatt, 2));
                        return VerhandlungsErgebnis.ArgumentAngenommen;
                    }
                    geduld--;
                    return VerhandlungsErgebnis.ArgumentAbgelehnt;
                }
                if (aktion == 3)
                {
                    double barFaktor = 0.88 + rnd.NextDouble() * 0.08;
                    preis = Math.Max(sk, Math.Round(preis * barFaktor, 2));
                    return VerhandlungsErgebnis.BarAngebotBerechnet;
                }
            }
            else
            {
                double skH = f.preis * (0.82 + rnd.NextDouble() * 0.06);
                if (aktion == 1)
                {
                    if (input > f.preis * 1.25) { geduld--; return VerhandlungsErgebnis.Ueberzogen; }
                    if (input <= skH) return VerhandlungsErgebnis.Deal;

                    geduld--;
                    double sprungFaktor = 0.3 + rnd.NextDouble() * 0.4;
                    preis = Math.Min(skH, Math.Round(preis + (input - preis) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Gegenvorschlag;
                }
                if (aktion == 2)
                {
                    preis = Math.Min(skH, Math.Round(preis + f.preis * (0.02 + rnd.NextDouble() * 0.05), 2));
                    return VerhandlungsErgebnis.ArgumentAngenommen;
                }
                if (aktion == 3) return VerhandlungsErgebnis.Deal;
            }
            return VerhandlungsErgebnis.Ungueltig;
        }
    }

    public enum VerhandlungsErgebnis
    {
        Deal, Frechheit, Entgegenkommen, ArgumentAngenommen, ArgumentAbgelehnt,
        BarAngebotBerechnet, Ueberzogen, Gegenvorschlag, Ungueltig
    }
}