using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Verwalter der Konsolen-Menüs und Benutzeroberfläche
    public class Menu
    {
        private readonly List<Haendler> haendlerListe;
        private readonly Kunde kunde;

        public Menu(List<Haendler> haendler, Kunde k) => (haendlerListe, kunde) = (haendler, k);

        // Die Haupt-Schleife der Anwendung zur Menüsteuerung
        public void starteHauptmenue()
        {
            while (true)
            {
                Console.Clear();
                zeigeHauptmenue(kunde);
                string wahl = leseEingabe("Option wählen: ").ToLower();

                if (wahl == "1")
                {
                    Console.Clear();
                    zeigeAlleHaendlerBestaende(haendlerListe);
                    Console.WriteLine("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                    Console.ReadKey();
                }
                else if (wahl == "2")
                {
                    verarbeiteKaufMenu();
                }
                else if (wahl == "3")
                {
                    Console.Clear();
                    kunde.garageAnzeigen();
                    Console.WriteLine("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                    Console.ReadKey();
                }
                else if (wahl == "4")
                {
                    verarbeiteVerkaufMenu();
                }
                else if (wahl == "x")
                {
                    Console.Clear();
                    Console.WriteLine("\nVielen Dank fürs Spielen! Programm wird beendet.");
                    break;
                }
                else
                {
                    printNachricht("Ungültige Wahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        // Untermenü: Kauf eines Fahrzeugs beim Händler
        private void verarbeiteKaufMenu()
        {
            while (true)
            {
                Console.Clear();
                zeigeHaendlerAuswahl(haendlerListe);
                string wahl = leseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int hIdx) && hIdx >= 1 && hIdx <= haendlerListe.Count)
                {
                    Haendler h = haendlerListe[hIdx - 1];
                    Console.Clear();
                    zeigeHaendlerBestand(h);
                    string fWahl = leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fWahl.ToLower() == "x") continue;

                    var bestand = h.getBestand();
                    if (int.TryParse(fWahl, out int fIdx) && fIdx >= 1 && fIdx <= bestand.Count)
                    {
                        Console.Clear();
                        // Verhandlung im VerhandlungsService ausführen
                        VerhandlungsService.fuehreKaufVerhandlung(h, bestand[fIdx - 1], kunde);
                        Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                        Console.ReadKey();
                        break;
                    }
                    printNachricht("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    printNachricht("Ungültige Händlerauswahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        // Untermenü: Verkauf eines eigenen Fahrzeugs an einen Händler
        private void verarbeiteVerkaufMenu()
        {
            if (kunde.fahrzeugGarage.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("\nDeine Garage ist leer. Du hast keine Fahrzeuge zum Verkaufen.\n");
                Console.WriteLine("Drücke eine beliebige Taste, um fortzufahren...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                zeigeHaendlerAuswahlVerkauf(haendlerListe);
                string wahl = leseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int vIdx) && vIdx >= 1 && vIdx <= haendlerListe.Count)
                {
                    Haendler h = haendlerListe[vIdx - 1];
                    Console.Clear();
                    kunde.garageAnzeigen();
                    string fWahl = leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fWahl.ToLower() == "x") continue;

                    if (int.TryParse(fWahl, out int fIdx) && fIdx >= 1 && fIdx <= kunde.fahrzeugGarage.Count)
                    {
                        Console.Clear();
                        // Ankaufverhandlung starten
                        VerhandlungsService.fuehreAnkaufVerhandlung(h, kunde.fahrzeugGarage[fIdx - 1], kunde);
                        Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                        Console.ReadKey();
                        break;
                    }
                    printNachricht("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        // Statische Anzeige- und Hilfsmethoden für die Ausgaben im Konsolenfenster
        public static void zeigeHauptmenue(Kunde k)
        {
            Console.WriteLine($"\n--- HAUPTMENÜ --- (Kunde: {k.vorname} {k.name} | Budget: {k.budget} EUR)");
            Console.WriteLine("[1] Alle Händler & Bestände anzeigen");
            Console.WriteLine("[2] Fahrzeug bei einem Händler kaufen (Verhandeln)");
            Console.WriteLine("[3] Eigene Garage anzeigen");
            Console.WriteLine("[4] Fahrzeug an einen Händler verkaufen");
            Console.WriteLine("[x] Programm beenden");
        }

        public static string leseEingabe(string text)
        {
            Console.Write(text);
            return Console.ReadLine();
        }

        public static void printNachricht(string msg) => Console.WriteLine($"\n[Hinweis] {msg}\n");

        public static void zeigeAlleHaendlerBestaende(List<Haendler> haendlerListe)
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

        public static void zeigeHaendlerAuswahl(List<Haendler> haendlerListe)
        {
            Console.WriteLine("\n--- HÄNDLER-AUSWAHL (KAUF) ---");
            for (int i = 0; i < haendlerListe.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName} (Geduld: {haendlerListe[i].geduld})");
            }
            Console.WriteLine("[x] Zurück");
        }

        public static void zeigeHaendlerAuswahlVerkauf(List<Haendler> haendlerListe)
        {
            Console.WriteLine("\n--- HÄNDLER-AUSWAHL (VERKAUF) ---");
            for (int i = 0; i < haendlerListe.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName}");
            }
            Console.WriteLine("[x] Zurück");
        }

        public static void zeigeHaendlerBestand(Haendler h)
        {
            Console.WriteLine($"\n--- Bestand von {h.haendlerName} ---");
            var bestand = h.getBestand();
            for (int i = 0; i < bestand.Count; i++)
            {
                Console.Write($"[{i + 1}] ");
                bestand[i].fahrzeugAnzeigen();
            }
        }
    }
}