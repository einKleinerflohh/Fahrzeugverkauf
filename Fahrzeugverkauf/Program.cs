using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Fahrzeugverkauf
{
    public class HaendlerDto
    {
        public string haendlerName { get; set; }
    }

    public class FahrzeugDto
    {
        public string typ { get; set; }
        public string haendlerName { get; set; }
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }
        public int anzahlSitzplaetze { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== FAHRZEUGVERKAUF & VERHANDLUNGSSYSTEM (JSON Edition) ===");

            List<Haendler> haendlerListe = new List<Haendler>();
            Random rnd = new Random();

            // Definition des Speicherorts: Ein Unterordner namens "Data" im Programmverzeichnis
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

            // Ordner automatisch erstellen, falls er nicht existiert
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string haendlerDatei = Path.Combine(dataDirectory, "haendler.json");
            string autoDatei = Path.Combine(dataDirectory, "auto.json");

            try
            {
                // 1. Händler aus Data/haendler.json laden und zufällige Geduld zuweisen
                if (File.Exists(haendlerDatei))
                {
                    string haendlerJson = File.ReadAllText(haendlerDatei);
                    var serializer = new JavaScriptSerializer();
                    var haendlerDtos = serializer.Deserialize<List<HaendlerDto>>(haendlerJson);

                    foreach (var hDto in haendlerDtos)
                    {
                        int zufallsGeduld = rnd.Next(2, 6);
                        haendlerListe.Add(new Haendler(hDto.haendlerName, zufallsGeduld));
                    }
                }
                else
                {
                    Console.WriteLine($"[Hinweis] '{haendlerDatei}' nicht gefunden. Standardwerte werden genutzt.");
                    haendlerListe.Add(new Haendler("Autohaus Müller", rnd.Next(2, 6)));
                    haendlerListe.Add(new Haendler("Gebrauchtwagen-Paradies", rnd.Next(2, 6)));
                }

                // 2. Fahrzeuge aus Data/auto.json laden und den Händlern zuordnen
                if (File.Exists(autoDatei))
                {
                    string autoJson = File.ReadAllText(autoDatei);
                    var serializer = new JavaScriptSerializer();
                    var fahrzeugDtos = serializer.Deserialize<List<FahrzeugDto>>(autoJson);

                    foreach (var fDto in fahrzeugDtos)
                    {
                        Fahrzeug neuesFahrzeug;
                        if (fDto.typ.Equals("Auto", StringComparison.OrdinalIgnoreCase))
                        {
                            neuesFahrzeug = new Auto(fDto.marke, fDto.modell, fDto.leistungInPS, fDto.tankInLiter, fDto.preis, fDto.anzahlSitzplaetze);
                        }
                        else
                        {
                            neuesFahrzeug = new Motorrad(fDto.marke, fDto.modell, fDto.leistungInPS, fDto.tankInLiter, fDto.preis);
                        }

                        var zielHaendler = haendlerListe.Find(h => h.haendlerName.Equals(fDto.haendlerName, StringComparison.OrdinalIgnoreCase));
                        if (zielHaendler != null)
                        {
                            zielHaendler.fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                        else if (haendlerListe.Count > 0)
                        {
                            haendlerListe[0].fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"[Hinweis] '{autoDatei}' nicht gefunden.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Fehler beim Laden der JSON-Dateien]: {ex.Message}");
            }

            Kunde kunde = new Kunde("Max", "Mustermann", 15000.0);

            Menu menu = new Menu(haendlerListe, kunde);
            menu.StarteHauptmenue();
        }

        public static void ZeigeHauptmenue(Kunde k)
        {
            Console.WriteLine($"\n--- HAUPTMENÜ --- (Kunde: {k.vorname} {k.name} | Budget: {k.budget} EUR)");
            Console.WriteLine("[1] Alle Händler & Bestände anzeigen");
            Console.WriteLine("[2] Fahrzeug bei einem Händler kaufen (Verhandeln)");
            Console.WriteLine("[3] Eigene Garage anzeigen");
            Console.WriteLine("[4] Fahrzeug an einen Händler verkaufen");
            Console.WriteLine("[x] Programm beenden");
        }

        public static string LeseEingabe(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        public static void PrintNachricht(string msg) => Console.WriteLine($"\n[Hinweis] {msg}\n");

        public static void ZeigeAlleHaendlerBestaende(List<Haendler> haendlerListe)
        {
            foreach (var h in haendlerListe)
            {
                Console.WriteLine($"\n--- Händler: {h.haendlerName} (Geduld: {h.geduld}) ---");
                var bestand = h.getBestand();
                if (bestand.Count == 0) Console.WriteLine("  Keine Fahrzeuge im Bestand.");
                else
                {
                    for (int i = 0; i < bestand.Count; i++)
                    {
                        Console.Write($"  [{i + 1}] ");
                        bestand[i].fahrzeugAnzeigen();
                    }
                }
            }
        }

        public static void ZeigeHaendlerAuswahl(List<Haendler> haendlerListe)
        {
            Console.WriteLine("\n--- HÄNDLER-AUSWAHL (KAUF) ---");
            for (int i = 0; i < haendlerListe.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName} (Geduld: {haendlerListe[i].geduld})");
            }
            Console.WriteLine("[x] Zurück");
        }

        public static void ZeigeHaendlerAuswahlVerkauf(List<Haendler> haendlerListe)
        {
            Console.WriteLine("\n--- HÄNDLER-AUSWAHL (VERKAUF) ---");
            for (int i = 0; i < haendlerListe.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName}");
            }
            Console.WriteLine("[x] Zurück");
        }

        public static void ZeigeHaendlerBestand(Haendler h)
        {
            Console.WriteLine($"\n--- Bestand von {h.haendlerName} ---");
            var bestand = h.getBestand();
            for (int i = 0; i < bestand.Count; i++)
            {
                Console.Write($"[{i + 1}] ");
                bestand[i].fahrzeugAnzeigen();
            }
        }

        public static void ZeigeGarage(Kunde k) => k.garageAnzeigen();
        public static void ZeigeGarageLeereMeldung() => Console.WriteLine("\nDeine Garage ist leer. Du hast keine Fahrzeuge zum Verkaufen.\n");
        public static void BeendenAusgabe() => Console.WriteLine("\nVielen Dank fürs Spielen! Programm wird beendet.");

        public static void FuehreKaufVerhandlung(Haendler h, Fahrzeug f, Kunde k)
        {
            int maxVersuche = h.GeneriereMaxVersuche();
            int verbleibendeGeduld = h.geduld;
            double aktuellerPreis = f.preis;

            Console.WriteLine($"\n=== Verhandlung mit {h.haendlerName} ===");
            Console.WriteLine($"Fahrzeug: {f.marke} {f.modell} | Preis: {aktuellerPreis} EUR");

            for (int runde = 1; runde <= maxVersuche; runde++)
            {
                Console.WriteLine($"\n--- Runde {runde} von {maxVersuche} (Geduld des Händlers: {verbleibendeGeduld}) ---");
                Console.WriteLine("Aktionen: [1] Gebot abgeben | [2] Argument anbringen | [3] Barzahlungs-Joker nutzen | [x] Abbrechen");
                string wahl = LeseEingabe("Wähle eine Aktion: ").ToLower();

                if (wahl == "x")
                {
                    Console.WriteLine("Du hast die Verhandlung abgebrochen.");
                    return;
                }

                if (!int.TryParse(wahl, out int aktion) || aktion < 1 || aktion > 3)
                {
                    Console.WriteLine("Ungültige Aktion.");
                    runde--;
                    continue;
                }

                double inputVal = 0;
                if (aktion == 1)
                {
                    string inputStr = LeseEingabe("Dein Preisvorschlag in EUR: ");
                    if (!double.TryParse(inputStr, out inputVal)) { Console.WriteLine("Ungültige Eingabe."); runde--; continue; }
                }

                VerhandlungsErgebnis ergebnis = h.VerarbeiteVerhandlung(true, f, aktion, inputVal, ref aktuellerPreis, ref verbleibendeGeduld);

                if (ergebnis == VerhandlungsErgebnis.Frechheit)
                {
                    Console.WriteLine($"\nDer Händler ist empört über dieses unverschämte Gebot und schmeißt dich raus! Verhandlung gescheitert.");
                    return;
                }
                if (ergebnis == VerhandlungsErgebnis.Deal)
                {
                    if (k.budget < aktuellerPreis)
                    {
                        Console.WriteLine($"\nDer Händler akzeptiert {aktuellerPreis} EUR, aber dein Budget reicht leider nicht aus!");
                        return;
                    }
                    k.budget -= aktuellerPreis;
                    h.fahrzeugEntfernen(f);
                    k.fahrzeugGarage.Add(f);
                    Console.WriteLine($"\nGlückwunsch! Kaufvertrag abgeschlossen für {aktuellerPreis} EUR. Das Fahrzeug wurde deiner Garage hinzugefügt.");
                    Console.WriteLine($"Neues Budget: {k.budget} EUR");
                    return;
                }
                if (ergebnis == VerhandlungsErgebnis.Entgegenkommen)
                {
                    Console.WriteLine($"Der Händler lässt sich auf einen neuen Preis ein: {aktuellerPreis} EUR.");
                }
                else if (ergebnis == VerhandlungsErgebnis.ArgumentAngenommen)
                {
                    Console.WriteLine($"Dein Argument hat gewirkt! Neuer Preis: {aktuellerPreis} EUR.");
                }
                else if (ergebnis == VerhandlungsErgebnis.ArgumentAbgelehnt)
                {
                    Console.WriteLine("Der Händler lässt sich von deinem Argument nicht beeindrucken und verliert die Geduld.");
                }
                else if (ergebnis == VerhandlungsErgebnis.BarAngebotBerechnet)
                {
                    Console.WriteLine($"Barzahlungs-Joker eingesetzt! Der Händler bietet sofort: {aktuellerPreis} EUR.");
                }

                if (verbleibendeGeduld <= 0)
                {
                    Console.WriteLine("\nDer Händler hat keine Geduld mehr und bricht das Gespräch ab!");
                    return;
                }
            }
            Console.WriteLine("\nKeine Einigung erzielt. Die Verhandlungsrunden sind aufgebraucht.");
        }

        public static void FuehreAnkaufVerhandlung(Haendler h, Fahrzeug f, Kunde k)
        {
            int maxVersuche = 3;
            int verbleibendeGeduld = h.geduld;
            double aktuellerPreis = f.preis * 0.75;

            Console.WriteLine($"\n=== Ankauf-Verhandlung mit {h.haendlerName} ===");
            Console.WriteLine($"Dein Fahrzeug: {f.marke} {f.modell} | Startgebot des Händlers: {aktuellerPreis} EUR");

            for (int runde = 1; runde <= maxVersuche; runde++)
            {
                Console.WriteLine($"\n--- Runde {runde} von {maxVersuche} ---");
                Console.WriteLine("Aktionen: [1] Gegenvorschlag nennen | [2] Zustand betonen (Scheckheft/TÜV) | [3] Sofortverkauf zum Händlerpreis | [x] Abbrechen");
                string wahl = LeseEingabe("Wähle eine Aktion: ").ToLower();

                if (wahl == "x") { Console.WriteLine("Verhandlung abgebrochen."); return; }

                if (!int.TryParse(wahl, out int aktion) || aktion < 1 || aktion > 3)
                {
                    Console.WriteLine("Ungültige Aktion.");
                    runde--;
                    continue;
                }

                double inputVal = 0;
                if (aktion == 1)
                {
                    string inputStr = LeseEingabe("Dein Wunschpreis in EUR: ");
                    if (!double.TryParse(inputStr, out inputVal)) { Console.WriteLine("Ungültige Eingabe."); runde--; continue; }
                }

                VerhandlungsErgebnis ergebnis = h.VerarbeiteVerhandlung(false, f, aktion, inputVal, ref aktuellerPreis, ref verbleibendeGeduld);

                if (ergebnis == VerhandlungsErgebnis.Ueberzogen)
                {
                    Console.WriteLine("Deine Preisvorstellung war völlig überzogen. Der Händler bricht das Gespräch ab.");
                    return;
                }
                if (ergebnis == VerhandlungsErgebnis.Deal)
                {
                    k.budget += aktuellerPreis;
                    k.fahrzeugGarage.Remove(f);
                    h.fahrzeugHinzufuegen(f);
                    Console.WriteLine($"\nAnkauf erfolgreich! Du hast das Fahrzeug für {aktuellerPreis} EUR an {h.haendlerName} verkauft.");
                    Console.WriteLine($"Neues Budget: {k.budget} EUR");
                    return;
                }
                if (ergebnis == VerhandlungsErgebnis.Gegenvorschlag)
                {
                    Console.WriteLine($"Der Händler macht einen neuen Gegenvorschlag: {aktuellerPreis} EUR.");
                }
                else if (ergebnis == VerhandlungsErgebnis.ArgumentAngenommen)
                {
                    Console.WriteLine($"Der Händler erkennt den guten Zustand an. Neuer Preis: {aktuellerPreis} EUR.");
                }

                if (verbleibendeGeduld <= 0)
                {
                    Console.WriteLine("\nDem Händler platzt der Kragen. Gespräch beendet.");
                    return;
                }
            }
            Console.WriteLine("\nKeine Einigung beim Ankauf erzielt.");
        }
    }
}