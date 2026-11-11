using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    public class Haendler
    {
        public string haendlerName { get; set; }
        public int geduld { get; set; }
        public string aktuelleStimmung { get; private set; }

        private readonly List<Fahrzeug> fahrzeugBestand = new List<Fahrzeug>();
        private readonly Random rnd = new Random();

        private readonly string[] stimmungen = { "Gut gelaunt", "Genervt", "Unberechenbar", "Streng", "Entspannt" };

        // Konstanten für Verhandlungsgrenzen und Faktoren
        private const double StimmungseinflussStark = 0.04;
        private const double StimmungseinflussZufallSpanne = 0.1;
        private const double StimmungseinflussZufallOffset = 0.05;

        private const double SchmerzgrenzeKaufBasis = 0.78;
        private const double SchmerzgrenzeKaufVarianz = 0.08;
        private const double FrechheitsgrenzeBasis = 0.65;
        private const double FrechheitsgrenzeVarianz = 0.06;

        private const double SprungFaktorMin = 0.3;
        private const double SprungFaktorSpanne = 0.4;

        private const double RabattArgumentMin = 0.02;
        private const double RabattArgumentSpanne = 0.06;

        private const double BarAngebotBasis = 0.88;
        private const double BarAngebotVarianz = 0.08;

        private const double SchmerzgrenzeAnkaufBasis = 0.82;
        private const double SchmerzgrenzeAnkaufVarianz = 0.06;
        private const double AnkaufUeberzogenLimit = 1.25;

        private const int MinVersucheRunden = 2;
        private const int MaxVersucheRunden = 6; // Obergrenze für Random.Next (exklusive)

        private const double StimmungChanceGutGelaunt = 0.8;
        private const double StimmungChanceGenervt = 0.3;
        private const double StimmungChanceStandard = 0.55;

        public Haendler() { }

        public Haendler(string name, int startGeduld)
        {
            haendlerName = name;
            geduld = startGeduld;
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
        }

        public void fahrzeugHinzufuegen(Fahrzeug fahrzeug) => fahrzeugBestand.Add(fahrzeug);
        public void fahrzeugEntfernen(Fahrzeug fahrzeug) => fahrzeugBestand.Remove(fahrzeug);
        public List<Fahrzeug> getBestand() => fahrzeugBestand;

        public int generiereMaxVersuche()
        {
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
            return rnd.Next(MinVersucheRunden, MaxVersucheRunden);
        }

        public VerhandlungsErgebnis verarbeiteVerhandlung(bool istKauf, Fahrzeug fahrzeug, int aktion, double input, ref double preis, ref int geduld)
        {
            if (istKauf)
            {
                double stimmungsFaktor = aktuelleStimmung == "Gut gelaunt" || aktuelleStimmung == "Entspannt" ? StimmungseinflussStark :
                                         (aktuelleStimmung == "Genervt" || aktuelleStimmung == "Streng" ? -StimmungseinflussStark : (rnd.NextDouble() * StimmungseinflussZufallSpanne - StimmungseinflussZufallOffset));

                double schmerzgrenze = fahrzeug.preis * (SchmerzgrenzeKaufBasis + rnd.NextDouble() * SchmerzgrenzeKaufVarianz + stimmungsFaktor);
                double frechheitsgrenze = fahrzeug.preis * (FrechheitsgrenzeBasis + rnd.NextDouble() * FrechheitsgrenzeVarianz);

                if (aktion == 1)
                {
                    if (input < frechheitsgrenze) return VerhandlungsErgebnis.Frechheit;
                    if (input >= preis) return VerhandlungsErgebnis.Deal;

                    geduld--;
                    double sprungFaktor = SprungFaktorMin + rnd.NextDouble() * SprungFaktorSpanne;
                    preis = Math.Max(schmerzgrenze, Math.Round(preis - (preis - input) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Entgegenkommen;
                }
                if (aktion == 2)
                {
                    bool erfolg = rnd.NextDouble() < (aktuelleStimmung == "Gut gelaunt" ? StimmungChanceGutGelaunt : (aktuelleStimmung == "Genervt" ? StimmungChanceGenervt : StimmungChanceStandard));
                    if (erfolg)
                    {
                        double rabatt = RabattArgumentMin + rnd.NextDouble() * RabattArgumentSpanne;
                        preis = Math.Max(schmerzgrenze, Math.Round(preis - fahrzeug.preis * rabatt, 2));
                        return VerhandlungsErgebnis.ArgumentAngenommen;
                    }
                    geduld--;
                    return VerhandlungsErgebnis.ArgumentAbgelehnt;
                }
                if (aktion == 3)
                {
                    double barFaktor = BarAngebotBasis + rnd.NextDouble() * BarAngebotVarianz;
                    preis = Math.Max(schmerzgrenze, Math.Round(preis * barFaktor, 2));
                    return VerhandlungsErgebnis.BarAngebotBerechnet;
                }
            }
            else
            {
                double schmerzgrenzeHaendler = fahrzeug.preis * (SchmerzgrenzeAnkaufBasis + rnd.NextDouble() * SchmerzgrenzeAnkaufVarianz);
                if (aktion == 1)
                {
                    if (input > fahrzeug.preis * AnkaufUeberzogenLimit) { geduld--; return VerhandlungsErgebnis.Ueberzogen; }
                    if (input <= schmerzgrenzeHaendler) return VerhandlungsErgebnis.Deal;

                    geduld--;
                    double sprungFaktor = SprungFaktorMin + rnd.NextDouble() * SprungFaktorSpanne;
                    preis = Math.Min(schmerzgrenzeHaendler, Math.Round(preis + (input - preis) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Gegenvorschlag;
                }
                if (aktion == 2)
                {
                    preis = Math.Min(schmerzgrenzeHaendler, Math.Round(preis + fahrzeug.preis * (RabattArgumentMin + rnd.NextDouble() * (RabattArgumentSpanne + 0.01)), 2));
                    return VerhandlungsErgebnis.ArgumentAngenommen;
                }
                if (aktion == 3)
                {
                    return VerhandlungsErgebnis.Deal;
                }
            }
            return VerhandlungsErgebnis.Ungueltig;
        }
    }

    public enum VerhandlungsErgebnis
    {
        Deal,
        Frechheit,
        Entgegenkommen,
        ArgumentAngenommen,
        ArgumentAbgelehnt,
        BarAngebotBerechnet,
        Ueberzogen,
        Gegenvorschlag,
        Ungueltig
    }
}