using System;

namespace Fahrzeugverkauf
{
    // Service-Klasse zur Steuerung der interaktiven Verhandlungsabläufe (Kauf und Ankauf)
    public static class VerhandlungsService
    {
        // Führt den Kaufprozess zwischen Kunde und Händler auf der Konsole durch
        public static void fuehreKaufVerhandlung(Haendler h, Fahrzeug f, Kunde k)
        {
            int maxVersuche = h.generiereMaxVersuche();
            int geduld = h.geduld;
            double aktuellerPreis = f.preis;

            Console.WriteLine($"\n=== Kaufverhandlung mit {h.haendlerName} ({h.aktuelleStimmung}) ===");
            Console.WriteLine($"Fahrzeug: {f.marke} {f.modell} | Ursprungspreis: {f.preis} EUR");

            for (int runde = 1; runde <= maxVersuche; runde++)
            {
                // Abbruch, wenn die Geduld des Händlers erschöpft ist
                if (geduld <= 0)
                {
                    Console.WriteLine($"\n[{h.haendlerName}]: Mir reicht's! Die Verhandlung ist beendet.");
                    return;
                }

                Console.WriteLine($"\n--- Runde {runde} | Aktueller Preis: {aktuellerPreis} EUR ---");
                Console.WriteLine("[1] Konkretes Gebot abgeben");
                Console.WriteLine("[2] Argument bringen (Zustand/Konkurrenz)");
                Console.WriteLine("[3] Barzahlungs-Joker nutzen");
                Console.WriteLine("[x] Verhandlung abbrechen");

                string wahl = Menu.leseEingabe("Deine Aktion: ").ToLower();
                if (wahl == "x")
                {
                    Console.WriteLine("\nVerhandlung abgebrochen.");
                    return;
                }

                int aktion = int.TryParse(wahl, out int a) ? a : 0;
                double gebot = 0;

                if (aktion == 1)
                {
                    double.TryParse(Menu.leseEingabe("Dein Gebot in EUR: "), out gebot);
                }

                // Logik im Händler verarbeiten
                VerhandlungsErgebnis ergebnis = h.verarbeiteVerhandlung(true, f, aktion, gebot, ref aktuellerPreis, ref geduld);

                // Ergebnis der Runde auswerten und anzeigen
                switch (ergebnis)
                {
                    case VerhandlungsErgebnis.Deal:
                        if (k.budget >= aktuellerPreis)
                        {
                            // Kauf erfolgreich abwickeln
                            k.budget -= aktuellerPreis;
                            k.budget = Math.Round(k.budget, 2);
                            h.fahrzeugEntfernen(f);
                            k.fahrzeugGarage.Add(f);
                            Console.WriteLine($"\nGlückwunsch! Du hast den {f.marke} {f.modell} für {aktuellerPreis} EUR gekauft.");
                            Console.WriteLine($"Neues Budget: {k.budget} EUR");
                        }
                        else
                        {
                            Console.WriteLine("\n[Händler]: Wir haben einen Deal, aber dein Budget reicht leider nicht aus!");
                        }
                        return;

                    case VerhandlungsErgebnis.Frechheit:
                        Console.WriteLine($"\n[{h.haendlerName}]: Das Gebot ist eine Frechheit! Auf Wiedersehen.");
                        return;

                    case VerhandlungsErgebnis.Entgegenkommen:
                        Console.WriteLine($"\n[{h.haendlerName}]: Dein Gebot ist zu niedrig. Ich biete dir {aktuellerPreis} EUR.");
                        break;

                    case VerhandlungsErgebnis.ArgumentAngenommen:
                        Console.WriteLine($"\n[{h.haendlerName}]: Gutes Argument! Neuer Preis: {aktuellerPreis} EUR.");
                        break;

                    case VerhandlungsErgebnis.ArgumentAbgelehnt:
                        Console.WriteLine($"\n[{h.haendlerName}]: Das Argument überzeugt mich nicht!");
                        break;

                    case VerhandlungsErgebnis.BarAngebotBerechnet:
                        Console.WriteLine($"\n[{h.haendlerName}]: Für Sofort-Barzahlung machen wir {aktuellerPreis} EUR!");
                        break;

                    default:
                        Console.WriteLine("\nUngültige Eingabe!");
                        runde--; // Runde nicht verzählen bei Ungültigkeit
                        break;
                }
            }
            Console.WriteLine($"\n[{h.haendlerName}]: Keine Einigung erzielt. Die Verhandlung ist beendet.");
        }

        // Führt den Ankaufprozess (Kunde verkauft Fahrzeug an Händler) auf der Konsole durch
        public static void fuehreAnkaufVerhandlung(Haendler h, Fahrzeug f, Kunde k)
        {
            int maxVersuche = h.generiereMaxVersuche();
            int geduld = h.geduld;
            double aktuellerPreis = Math.Round(f.preis * 0.7, 2); // Händler beginnt mit niedrigem Ankaufpreis

            Console.WriteLine($"\n=== Ankaufverhandlung mit {h.haendlerName} ===");
            Console.WriteLine($"Dein Fahrzeug: {f.marke} {f.modell} | Startgebot Händler: {aktuellerPreis} EUR");

            for (int runde = 1; runde <= maxVersuche; runde++)
            {
                if (geduld <= 0)
                {
                    Console.WriteLine($"\n[{h.haendlerName}]: Mir reicht's! Ich werde das Fahrzeug nicht kaufen.");
                    return;
                }

                Console.WriteLine($"\n--- Runde {runde} | Aktuelles Händlerangebot: {aktuellerPreis} EUR ---");
                Console.WriteLine("[1] Gegenangebot (Preisvorstellung) nennen");
                Console.WriteLine("[2] Fahrzeugvorteil hervorheben (z.B. Scheckheft, neuer TÜV)");
                Console.WriteLine("[3] Angebot des Händlers akzeptieren");
                Console.WriteLine("[x] Verhandlung abbrechen");

                string wahl = Menu.leseEingabe("Deine Aktion: ").ToLower();
                if (wahl == "x")
                {
                    Console.WriteLine("\nVerhandlung abgebrochen.");
                    return;
                }

                int aktion = int.TryParse(wahl, out int a) ? a : 0;
                double gebot = 0;

                if (aktion == 1)
                {
                    double.TryParse(Menu.leseEingabe("Deine Preisvorstellung in EUR: "), out gebot);
                }

                VerhandlungsErgebnis ergebnis = h.verarbeiteVerhandlung(false, f, aktion, gebot, ref aktuellerPreis, ref geduld);

                switch (ergebnis)
                {
                    case VerhandlungsErgebnis.Deal:
                        // Verkaufen & Erlös gutschreiben
                        k.budget += aktuellerPreis;
                        k.budget = Math.Round(k.budget, 2);
                        k.fahrzeugGarage.Remove(f);
                        h.fahrzeugHinzufuegen(f);
                        Console.WriteLine($"\nVerkauft! Du hast deinen {f.marke} {f.modell} für {aktuellerPreis} EUR an {h.haendlerName} verkauft.");
                        Console.WriteLine($"Neues Budget: {k.budget} EUR");
                        return;

                    case VerhandlungsErgebnis.Ueberzogen:
                        Console.WriteLine($"\n[{h.haendlerName}]: Das ist völlig überzogen!");
                        break;

                    case VerhandlungsErgebnis.Gegenvorschlag:
                        Console.WriteLine($"\n[{h.haendlerName}]: Das ist mir zu viel. Neues Angebot: {aktuellerPreis} EUR.");
                        break;

                    case VerhandlungsErgebnis.ArgumentAngenommen:
                        Console.WriteLine($"\n[{h.haendlerName}]: Überzeugt! Ich erhöhe mein Angebot auf {aktuellerPreis} EUR.");
                        break;

                    default:
                        Console.WriteLine("\nUngültige Eingabe!");
                        runde--;
                        break;
                }
            }
            Console.WriteLine($"\n[{h.haendlerName}]: Wir konnten uns nicht auf einen Preis einigen.");
        }
    }
}
