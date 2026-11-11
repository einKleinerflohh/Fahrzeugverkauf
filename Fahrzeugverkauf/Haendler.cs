using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Repräsentiert einen Autohändler mit eigenem Fahrzeugbestand, Gedulds- und Stimmungssystem
    public class Haendler
    {
        public string haendlerName { get; set; }
        public int geduld { get; set; }
        public string aktuelleStimmung { get; private set; }

        // Interner Fahrzeugbestand des Händlers
        private readonly List<Fahrzeug> fahrzeugBestand = new List<Fahrzeug>();
        private readonly Random rnd = new Random();

        // Mögliche Stimmungen, die das Verhandlungsverhalten beeinflussen
        private readonly string[] stimmungen = { "Gut gelaunt", "Genervt", "Unberechenbar", "Streng", "Entspannt" };

        public Haendler() { }

        public Haendler(string name, int startGeduld)
        {
            haendlerName = name;
            geduld = startGeduld;
            // Zufällige Stimmung beim Erstellen zuweisen
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
        }

        // Methoden zur Bestandsverwaltung
        public void fahrzeugHinzufuegen(Fahrzeug f) => fahrzeugBestand.Add(f);
        public void fahrzeugEntfernen(Fahrzeug f) => fahrzeugBestand.Remove(f);
        public List<Fahrzeug> getBestand() => fahrzeugBestand;

        // Generiert die maximale Anzahl der Verhandlungsrunden für ein Gespräch und wechselt die Stimmung
        public int generiereMaxVersuche()
        {
            aktuelleStimmung = stimmungen[rnd.Next(stimmungen.Length)];
            return rnd.Next(2, 6);
        }

        // Zentrale Verhandlungslogik für Kauf- und Ankaufgespräche.
        // Berechnet Preisänderungen, Geduldsabzug und Verhandlungsergebnisse basierend auf der Kundenaktion.
        public VerhandlungsErgebnis verarbeiteVerhandlung(bool istKauf, Fahrzeug f, int aktion, double input, ref double preis, ref int geduld)
        {
            if (istKauf)
            {
                // Kaufverhandlung (Kunde möchte ein Fahrzeug vom Händler kaufen)

                // Errechnung des Stimmungseinflusses auf die Schmerzgrenze
                double stimmungsFaktor = aktuelleStimmung == "Gut gelaunt" || aktuelleStimmung == "Entspannt" ? 0.04 :
                                         (aktuelleStimmung == "Genervt" || aktuelleStimmung == "Streng" ? -0.04 : (rnd.NextDouble() * 0.1 - 0.05));

                // Schmerzgrenze (sk) und Frechheitsgrenze (fk) des Händlers berechnen
                double sk = f.preis * (0.78 + rnd.NextDouble() * 0.08 + stimmungsFaktor);
                double fk = f.preis * (0.65 + rnd.NextDouble() * 0.06);

                if (aktion == 1) // Konkretes Gebot
                {
                    if (input < fk) return VerhandlungsErgebnis.Frechheit; // Zu niedrig: Händler bricht ab
                    if (input >= preis) return VerhandlungsErgebnis.Deal; // Gebot erreicht geforderten Preis

                    geduld--; // Geduld sinkt bei noch nicht akzeptiertem Gebot
                    double sprungFaktor = 0.3 + rnd.NextDouble() * 0.4;
                    // Händler kommt dem Kunden preislich entgegen
                    preis = Math.Max(sk, Math.Round(preis - (preis - input) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Entgegenkommen;
                }
                if (aktion == 2) // Argument vorbringen
                {
                    // Wahrscheinlichkeit des Erfolgs hängt von der aktuellen Stimmung ab
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
                if (aktion == 3) // Barzahlungs-Joker
                {
                    double barFaktor = 0.88 + rnd.NextDouble() * 0.08;
                    preis = Math.Max(sk, Math.Round(preis * barFaktor, 2));
                    return VerhandlungsErgebnis.BarAngebotBerechnet;
                }
            }
            else
            {
                // Ankaufverhandlung (Kunde möchte sein Fahrzeug an den Händler verkaufen)

                double skH = f.preis * (0.82 + rnd.NextDouble() * 0.06); // Schmerzgrenze des Händlers beim Ankauf
                if (aktion == 1) // Preisvorstellung des Kunden
                {
                    if (input > f.preis * 1.25) { geduld--; return VerhandlungsErgebnis.Ueberzogen; } // Übertriebener Preis
                    if (input <= skH) return VerhandlungsErgebnis.Deal; // Händler akzeptiert das Angebot

                    geduld--;
                    double sprungFaktor = 0.3 + rnd.NextDouble() * 0.4;
                    // Händler erhöht sein Ankaufgebot schrittweise
                    preis = Math.Min(skH, Math.Round(preis + (input - preis) * sprungFaktor, 2));
                    return VerhandlungsErgebnis.Gegenvorschlag;
                }
                if (aktion == 2) // Argument für höheren Preis
                {
                    preis = Math.Min(skH, Math.Round(preis + f.preis * (0.02 + rnd.NextDouble() * 0.05), 2));
                    return VerhandlungsErgebnis.ArgumentAngenommen;
                }
                if (aktion == 3) // Händler-Angebot direkt akzeptieren
                {
                    return VerhandlungsErgebnis.Deal;
                }
            }
            return VerhandlungsErgebnis.Ungueltig;
        }
    }

    // Enum zur Verhandlungs-Auswertung
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