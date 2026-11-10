using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fahrzeugverkauf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Hinweis: Dieses Programm ist vollständig fiktional inklusive aller Namen, Adressen und Kontostände.

            // Händler-Instanzen mit unterschiedlichen Namen und individuellen Geduldswerten anlegen
            Haendler haendler1 = new Haendler("Autohaus Müller", 2);          // Geringe Geduld (2)
            Haendler haendler2 = new Haendler("Gebrauchtwagen-Paradies", 3); // Normale Geduld (3)
            Haendler haendler3 = new Haendler("Zweirad & Auto Schmidt", 5);  // Hohe Geduld (5)

            List<Haendler> haendlerListe = new List<Haendler> { haendler1, haendler2, haendler3 };

            // Befüllen des Fahrzeugbestands für Händler 1
            haendler1.fahrzeugHinzufuegen(new Auto("Audi", "A4", 190, 50.0, 32000.0, 5));
            haendler1.fahrzeugHinzufuegen(new Auto("BMW", "M3", 480, 60.0, 75000.0, 5));
            haendler1.fahrzeugHinzufuegen(new Auto("Porsche", "911 Carrera", 385, 64.0, 115000.0, 2));
            haendler1.fahrzeugHinzufuegen(new Motorrad("Yamaha", "MT-07", 73.4, 14.0, 7999.0));

            // Befüllen des Fahrzeugbestands für Händler 2
            haendler2.fahrzeugHinzufuegen(new Auto("Volkswagen", "Golf VII", 150, 50.0, 18500.0, 5));
            haendler2.fahrzeugHinzufuegen(new Auto("Skoda", "Octavia", 150, 55.0, 21000.0, 5));
            haendler2.fahrzeugHinzufuegen(new Auto("Fiat", "500", 69, 35.0, 9500.0, 4));
            haendler2.fahrzeugHinzufuegen(new Motorrad("Honda", "Forza 75", 58.6, 13.2, 13147.00));
            haendler2.fahrzeugHinzufuegen(new Motorrad("BMW", "R 1250 GS", 136, 20.0, 18500.0));

            // Befüllen des Fahrzeugbestands für Händler 3
            haendler3.fahrzeugHinzufuegen(new Motorrad("Aprilia", "RS 660", 100, 15.0, 11000.0));
            haendler3.fahrzeugHinzufuegen(new Motorrad("Kawasaki", "Z900", 125, 17.0, 9800.0));
            haendler3.fahrzeugHinzufuegen(new Auto("Mercedes", "C 220", 194, 66.0, 29999.0, 5));
            haendler3.fahrzeugHinzufuegen(new Auto("Tesla", "Model 3", 325, 50.0, 42000.0, 5));

            // Programmstart: Kundendaten erfassen
            Console.Clear();
            Console.WriteLine("=== WILLKOMMEN BEIM FAHRZEUGVERKAUF ===");

            Console.Write("Bitte gib deinen Vornamen ein: ");
            string kundenVorname = Console.ReadLine();
            Console.Write("Bitte gib deinen Nachnamen ein: ");
            string kundenName = Console.ReadLine();

            double kundenBudget = 0;
            bool gueltigesBudget = false;

            while (!gueltigesBudget)
            {
                Console.Write("Bitte gib dein verfügbares Budget in Euro ein (z. B. 45000): ");
                string budgetEingabe = Console.ReadLine();

                if (double.TryParse(budgetEingabe, out kundenBudget) && kundenBudget >= 0)
                {
                    gueltigesBudget = true;
                }
                else
                {
                    Console.WriteLine("Ungültige Eingabe. Bitte gib eine gültige Zahl für dein Budget ein.");
                }
            }

            // Erstellen des Kunden-Objekts (beinhaltet nun standardmäßig auch eine Startgarage)
            Kunde kunde = new Kunde(kundenVorname, kundenName, kundenBudget);
            Console.WriteLine($"\nKunde erfolgreich angelegt!
            Thread.Sleep(2000);

            bool beenden = false;

            // Hauptmenü-Schleife
            while (!beenden)
            {
                Console.Clear();
                Console.WriteLine("\n=== FAHRZEUGVERKAUF - HAUPTMENÜ ===");
                Console.WriteLine($"Hallo {kunde.vorname} {kunde.name}, dein Budget beträgt {kunde.budget} EUR. Fahrzeuge in Garage: {kunde.fahrzeugGarage.Count}");
                Console.WriteLine("[1] Alle Händler und deren Bestände anzeigen");
                Console.WriteLine("[2] Händler auswählen und Fahrzeug KAUFEN (verhandeln)");
                Console.WriteLine("[3] Eigene Fahrzeuggarage anzeigen");
                Console.WriteLine("[4] Fahrzeug an einen Händler VERKAUFEN (verhandeln)");
                Console.WriteLine("[x] Beenden");
                Console.Write("Bitte wähle eine Option: ");

                string menueEingabe = Console.ReadLine();

                switch (menueEingabe)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("\n=== ÜBERSICHT ALLER HÄNDLER UND ANGEBOTE ===");
                        foreach (var h in haendlerListe)
                        {
                            h.bestandAnzeigen();
                        }
                        Console.WriteLine("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                        Console.ReadKey();
                        break;

                    case "2": // Fahrzeug kaufen
                        bool haendlerZurueck = false;
                        while (!haendlerZurueck)
                        {
                            Console.Clear();
                            Console.WriteLine("\n=== HÄNDLER-AUSWAHL (FAHRZEUG KAUFEN) ===");
                            for (int i = 0; i < haendlerListe.Count; i++)
                            {
                                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName} (Geduld: {haendlerListe[i].geduld})");
                            }
                            Console.WriteLine("[x] Zurück zum Hauptmenü");
                            Console.Write("Wähle einen Händler aus: ");

                            string hAuswahlEingabe = Console.ReadLine();

                            if (hAuswahlEingabe.ToLower() == "x")
                            {
                                haendlerZurueck = true;
                                break;
                            }

                            if (int.TryParse(hAuswahlEingabe, out int hIndex) && hIndex >= 1 && hIndex <= haendlerListe.Count)
                            {
                                Haendler gewaehlterHaendler = haendlerListe[hIndex - 1];
                                bool fahrzeugZurueck = false;

                                while (!fahrzeugZurueck)
                                {
                                    Console.Clear();
                                    gewaehlterHaendler.bestandAnzeigen();
                                    Console.WriteLine($"Dein Budget: {kunde.budget} EUR.");
                                    Console.WriteLine("Gib die Nummer des Fahrzeugs ein, das du kaufen möchtest.");
                                    Console.WriteLine("Gib 'x' ein, um zur Händler-Auswahl zurückzukehren.");
                                    Console.Write("\nEingabe: ");

                                    string fahrzeugEingabe = Console.ReadLine();

                                    if (fahrzeugEingabe.ToLower() == "x")
                                    {
                                        fahrzeugZurueck = true;
                                    }
                                    else
                                    {
                                        List<Fahrzeug> bestand = gewaehlterHaendler.getBestand();

                                        if (int.TryParse(fahrzeugEingabe, out int fIndex) && fIndex >= 1 && fIndex <= bestand.Count)
                                        {
                                            Fahrzeug gewaehltesFahrzeug = bestand[fIndex - 1];
                                            kunde.preisvorschlagAbgeben(gewaehltesFahrzeug, gewaehlterHaendler);

                                            Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                                            Console.ReadKey();
                                            fahrzeugZurueck = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ungültige Fahrzeugnummer. Bitte versuche es erneut.");
                                            Thread.Sleep(2000);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ungültige Händlerauswahl. Bitte versuche es erneut.");
                                Thread.Sleep(2000);
                            }
                        }
                        break;

                    case "3": // Eigene Garage anzeigen
                        Console.Clear();
                        kunde.garageAnzeigen();
                        Console.WriteLine("Drücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                        Console.ReadKey();
                        break;

                    case "4": // Fahrzeug an Händler verkaufen
                        if (kunde.fahrzeugGarage.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("\nDeine Garage ist leer. Du hast aktuell kein Fahrzeug, das du verkaufen könntest!");
                            Console.WriteLine("\nDrücke eine beliebige Taste...");
                            Console.ReadKey();
                            break;
                        }

                        bool verkaufHaendlerZurueck = false;
                        while (!verkaufHaendlerZurueck)
                        {
                            Console.Clear();
                            Console.WriteLine("\n=== HÄNDLER-AUSWAHL (FAHRZEUG VERKAUFEN) ===");
                            for (int i = 0; i < haendlerListe.Count; i++)
                            {
                                Console.WriteLine($"[{i + 1}] {haendlerListe[i].haendlerName}");
                            }
                            Console.WriteLine("[x] Zurück zum Hauptmenü");
                            Console.Write("Wähle einen Händler für den Verkauf aus: ");

                            string vHaendlerEingabe = Console.ReadLine();

                            if (vHaendlerEingabe.ToLower() == "x")
                            {
                                verkaufHaendlerZurueck = true;
                                break;
                            }

                            if (int.TryParse(vHaendlerEingabe, out int vHIndex) && vHIndex >= 1 && vHIndex <= haendlerListe.Count)
                            {
                                Haendler verkaufsHaendler = haendlerListe[vHIndex - 1];
                                bool fahrzeugWahlZurueck = false;

                                while (!fahrzeugWahlZurueck)
                                {
                                    Console.Clear();
                                    kunde.garageAnzeigen();
                                    Console.WriteLine("Gib die Nummer des Fahrzeugs aus deiner Garage ein, das du verkaufen möchtest.");
                                    Console.WriteLine("Gib 'x' ein, um zurückzukehren.");
                                    Console.Write("\nEingabe: ");

                                    string kFahrzeugEingabe = Console.ReadLine();

                                    if (kFahrzeugEingabe.ToLower() == "x")
                                    {
                                        fahrzeugWahlZurueck = true;
                                    }
                                    else
                                    {
                                        if (int.TryParse(kFahrzeugEingabe, out int kFIndex) && kFIndex >= 1 && kFIndex <= kunde.fahrzeugGarage.Count)
                                        {
                                            Fahrzeug zuVerkaufendesFahrzeug = kunde.fahrzeugGarage[kFIndex - 1];
                                            kunde.fahrzeugVerkaufen(zuVerkaufendesFahrzeug, verkaufsHaendler);

                                            Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                                            Console.ReadKey();
                                            fahrzeugWahlZurueck = true;
                                            verkaufHaendlerZurueck = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Ungültige Fahrzeugnummer. Bitte versuche es erneut.");
                                            Thread.Sleep(2000);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Ungültige Händlerauswahl.");
                                Thread.Sleep(2000);
                            }
                        }
                        break;

                    case "x":
                        beenden = true;
                        Console.Clear();
                        Console.WriteLine("\nProgramm wird beendet. Auf Wiedersehen!");
                        Thread.Sleep(2000);
                        break;

                    default:
                        Console.WriteLine("\nUngültige Wahl. Bitte wähle eine der vorgegebenen Optionen.");
                        Thread.Sleep(2000);
                        break;
                }
            }
        }
    }
}