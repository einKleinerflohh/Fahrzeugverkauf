using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Verwalter der Konsolen-Menüs und Benutzeroberfläche
    public class Menu
    {
        private readonly List<Haendler> haendlerListe;
        private readonly Kunde kunde;

        public Menu(List<Haendler> haendlerListeEingabe, Kunde kundeEingabe) => (haendlerListe, kunde) = (haendlerListeEingabe, kundeEingabe);

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

                if (int.TryParse(wahl, out int haendlerIndex) && haendlerIndex >= 1 && haendlerIndex <= haendlerListe.Count)
                {
                    Haendler ausgewaehlterHaendler = haendlerListe[haendlerIndex - 1];
                    Console.Clear();
                    zeigeHaendlerBestand(ausgewaehlterHaendler);
                    string fahrzeugWahl = leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fahrzeugWahl.ToLower() == "x") continue;

                    var bestand = ausgewaehlterHaendler.getBestand();
                    if (int.TryParse(fahrzeugWahl, out int fahrzeugIndex) && fahrzeugIndex >= 1 && fahrzeugIndex <= bestand.Count)
                    {
                        Console.Clear();
                        // Verhandlung im VerhandlungsService ausführen
                        VerhandlungsService.fuehreKaufVerhandlung(ausgewaehlterHaendler, bestand[fahrzeugIndex - 1], kunde);
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

                if (int.TryParse(wahl, out int verkaufIndex) && verkaufIndex >= 1 && verkaufIndex <= haendlerListe.Count)
                {
                    Haendler ausgewaehlterHaendler = haendlerListe[verkaufIndex - 1];
                    Console.Clear();
                    kunde.garageAnzeigen();
                    string fahrzeugWahl = leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fahrzeugWahl.ToLower() == "x") continue;

                    if (int.TryParse(fahrzeugWahl, out int fahrzeugIndex) && fahrzeugIndex >= 1 && fahrzeugIndex <= kunde.fahrzeugGarage.Count)
                    {
                        Console.Clear();
                        // Ankaufverhandlung starten
                        VerhandlungsService.fuehreAnkaufVerhandlung(ausgewaehlterHaendler, kunde.fahrzeugGarage[fahrzeugIndex - 1], kunde);
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
        public static void zeigeHauptmenue(Kunde kundeObjekt)
        {
            Console.WriteLine($"\n--- HAUPTMENÜ --- (Kunde: {kundeObjekt.vorname} {kundeObjekt.name} | Budget: {kundeObjekt.budget} EUR)");
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

        public static void printNachricht(string nachricht) => Console.WriteLine($"\n[Hinweis] {nachricht}\n");

        public static void zeigeAlleHaendlerBestaende(List<Haendler> haendlerListe)
        {
            foreach (var haendler in haendlerListe)
            {
                Console.WriteLine($"\n--- Händler: {haendler.haendlerName} ---");
                var bestand = haendler.getBestand();
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
                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName}");
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

        public static void zeigeHaendlerBestand(Haendler haendler)
        {
            Console.WriteLine($"\n--- Bestand von {haendler.haendlerName} ---");
            var bestand = haendler.getBestand();
            for (int i = 0; i < bestand.Count; i++)
            {
                Console.Write($"[{i + 1}] ");
                bestand[i].fahrzeugAnzeigen();
            }
        }
    }
}