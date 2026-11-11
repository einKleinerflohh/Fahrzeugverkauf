using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    public static class Dialog
    {
        public static void zeigeHauptmenue(Kunde kundeObjekt)
        {
            Console.Clear();
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

        public static void printHinweis(string nachricht)
        {
            Console.WriteLine($"\n[Hinweis] {nachricht}\n");
        }

        public static void printNachricht(string nachricht)
        {
            Console.WriteLine($"\n[Hinweis] {nachricht}\n");
        }

        public static void warteAufTaste(string text = "\nDrücke eine beliebige Taste, um fortzufahren...")
        {
            Console.WriteLine(text);
            Console.ReadKey();
        }

        public static void zeigeFahrzeugDetails(string marke, string modell, double leistungInPS, double tankInLiter, double preis, string zustand)
        {
            Console.Write($"Marke: {marke}, Modell: {modell}, Leistung: {leistungInPS} PS, Tank: {tankInLiter} L, Preis: {preis} EUR (Zustand: {zustand})");
        }

        public static void zeigeAutoDetails(int anzahlSitzplaetze)
        {
            Console.WriteLine($" -> Fahrzeugtyp: Auto, Sitzplätze: {anzahlSitzplaetze}");
        }

        public static void zeigeMotorradDetails()
        {
            Console.WriteLine(" -> Fahrzeugtyp: Motorrad");
        }

        public static void zeigeGarageTitel(string vorname, string name, double budget)
        {
            Console.WriteLine($"\n--- Fahrzeuggarage von {vorname} {name} (Budget: {budget} EUR) ---");
        }

        public static void zeigeGarageLeer()
        {
            Console.WriteLine("Deine Garage ist aktuell leer.");
            Console.WriteLine("--------------------------------------");
        }

        public static void zeigeGarageTrennlinie()
        {
            Console.WriteLine("--------------------------------------\n");
        }

        public static void zeigeListeneintrag(int index)
        {
            Console.Write($"[{index}] ");
        }

        public static void zeigeAlleHaendlerBestaende(List<Haendler> haendlerListe)
        {
            foreach (var haendler in haendlerListe)
            {
                Console.WriteLine($"\n--- Händler: {haendler.haendlerName} ---");
                var bestand = haendler.getBestand();
                if (bestand.Count == 0)
                {
                    Console.WriteLine("  Keine Fahrzeuge im Bestand.");
                }
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

        public static void zeigeHaendlerAuswahlKauf(List<Haendler> haendlerListe)
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

        public static void zeigeProgrammEnde()
        {
            Console.Clear();
            Console.WriteLine("\nVielen Dank fürs Spielen! Programm wird beendet.");
        }

        public static void zeigeGarageLeerVerkauf()
        {
            Console.Clear();
            Console.WriteLine("\nDeine Garage ist leer. Du hast keine Fahrzeuge zum Verkaufen.\n");
        }

        public static void zeigeJsonFehler(string datei, string meldung)
        {
            Console.WriteLine($"[Hinweis] '{datei}' nicht gefunden. Standardwerte werden genutzt.");
        }

        public static void zeigeAllgemeinerFehler(string fehlerMeldung)
        {
            Console.WriteLine($"[Fehler beim Laden der JSON-Dateien]: {fehlerMeldung}");
        }
    }
}
