using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    class Menu
    {
        private List<Haendler> haendlerListe;
        private Kunde kunde;

        public Menu(List<Haendler> haendler, Kunde k) => (haendlerListe, kunde) = (haendler, k);

        public void StarteHauptmenue()
        {
            while (true)
            {
                Console.Clear();
                Program.ZeigeHauptmenue(kunde);
                string wahl = Program.LeseEingabe("Option wählen: ").ToLower();

                if (wahl == "1")
                {
                    Console.Clear();
                    Program.ZeigeAlleHaendlerBestaende(haendlerListe);
                    Console.WriteLine("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                    Console.ReadKey();
                }
                else if (wahl == "2")
                {
                    VerarbeiteKaufMenu();
                }
                else if (wahl == "3")
                {
                    Console.Clear();
                    Program.ZeigeGarage(kunde);
                    Console.WriteLine("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                    Console.ReadKey();
                }
                else if (wahl == "4")
                {
                    VerarbeiteVerkaufMenu();
                }
                else if (wahl == "x")
                {
                    Console.Clear();
                    Program.BeendenAusgabe();
                    break;
                }
                else
                {
                    Program.PrintNachricht("Ungültige Wahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void VerarbeiteKaufMenu()
        {
            while (true)
            {
                Console.Clear();
                Program.ZeigeHaendlerAuswahl(haendlerListe);
                string wahl = Program.LeseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int hIdx) && hIdx >= 1 && hIdx <= haendlerListe.Count)
                {
                    Haendler h = haendlerListe[hIdx - 1];
                    Console.Clear();
                    Program.ZeigeHaendlerBestand(h);
                    string fWahl = Program.LeseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fWahl.ToLower() == "x") continue;

                    var bestand = h.getBestand();
                    if (int.TryParse(fWahl, out int fIdx) && fIdx >= 1 && fIdx <= bestand.Count)
                    {
                        Console.Clear();
                        Program.FuehreKaufVerhandlung(h, bestand[fIdx - 1], kunde);
                        Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                        Console.ReadKey();
                        break;
                    }
                    Program.PrintNachricht("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    Program.PrintNachricht("Ungültige Händlerauswahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void VerarbeiteVerkaufMenu()
        {
            if (kunde.fahrzeugGarage.Count == 0)
            {
                Console.Clear();
                Program.ZeigeGarageLeereMeldung();
                Console.WriteLine("Drücke eine beliebige Taste, um fortzufahren...");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                Program.ZeigeHaendlerAuswahlVerkauf(haendlerListe);
                string wahl = Program.LeseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int vIdx) && vIdx >= 1 && vIdx <= haendlerListe.Count)
                {
                    Haendler h = haendlerListe[vIdx - 1];
                    Console.Clear();
                    Program.ZeigeGarage(kunde);
                    string fWahl = Program.LeseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fWahl.ToLower() == "x") continue;

                    if (int.TryParse(fWahl, out int fIdx) && fIdx >= 1 && fIdx <= kunde.fahrzeugGarage.Count)
                    {
                        Console.Clear();
                        Program.FuehreAnkaufVerhandlung(h, kunde.fahrzeugGarage[fIdx - 1], kunde);
                        Console.WriteLine("\nDrücke eine beliebige Taste, um fortzufahren...");
                        Console.ReadKey();
                        break;
                    }
                    Program.PrintNachricht("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}