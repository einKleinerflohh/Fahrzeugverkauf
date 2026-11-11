using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    public class Menu
    {
        private readonly List<Haendler> haendlerListe;
        private readonly Kunde kunde;

        public Menu(List<Haendler> haendlerListeEingabe, Kunde kundeEingabe) => (haendlerListe, kunde) = (haendlerListeEingabe, kundeEingabe);

        public void starteHauptmenue()
        {
            while (true)
            {
                Console.Clear();
                Dialog.zeigeHauptmenue(kunde);
                string wahl = Dialog.leseEingabe("Option wählen: ").ToLower();

                if (wahl == "1")
                {
                    Console.Clear();
                    Dialog.zeigeAlleHaendlerBestaende(haendlerListe);
                    Dialog.warteAufTaste("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                }
                else if (wahl == "2")
                {
                    Console.Clear();
                    verarbeiteKaufMenu();
                }
                else if (wahl == "3")
                {
                    Console.Clear();
                    kunde.garageAnzeigen();
                    Dialog.warteAufTaste("\nDrücke eine beliebige Taste, um zum Hauptmenü zurückzukehren...");
                }
                else if (wahl == "4")
                {
                    Console.Clear();
                    verarbeiteVerkaufMenu();
                }
                else if (wahl == "x")
                {
                    Console.Clear();
                    Dialog.zeigeProgrammEnde();
                    break;
                }
                else
                {
                    Dialog.printHinweis("Ungültige Wahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void verarbeiteKaufMenu()
        {
            while (true)
            {
                Console.Clear();
                Dialog.zeigeHaendlerAuswahlKauf(haendlerListe);
                string wahl = Dialog.leseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int haendlerIndex) && haendlerIndex >= 1 && haendlerIndex <= haendlerListe.Count)
                {
                    Haendler ausgewaehlterHaendler = haendlerListe[haendlerIndex - 1];
                    Console.Clear();
                    Dialog.zeigeHaendlerBestand(ausgewaehlterHaendler);
                    string fahrzeugWahl = Dialog.leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fahrzeugWahl.ToLower() == "x") continue;

                    var bestand = ausgewaehlterHaendler.getBestand();
                    if (int.TryParse(fahrzeugWahl, out int fahrzeugIndex) && fahrzeugIndex >= 1 && fahrzeugIndex <= bestand.Count)
                    {
                        Console.Clear();
                        VerhandlungsService.fuehreKaufVerhandlung(ausgewaehlterHaendler, bestand[fahrzeugIndex - 1], kunde);
                        Dialog.warteAufTaste("\nDrücke eine beliebige Taste, um fortzufahren...");
                        break;
                    }
                    Dialog.printHinweis("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    Dialog.printHinweis("Ungültige Händlerauswahl.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void verarbeiteVerkaufMenu()
        {
            if (kunde.fahrzeugGarage.Count == 0)
            {
                Dialog.zeigeGarageLeerVerkauf();
                Dialog.warteAufTaste("Drücke eine beliebige Taste, um fortzufahren...");
                return;
            }

            while (true)
            {
                Console.Clear();
                Dialog.zeigeHaendlerAuswahlVerkauf(haendlerListe);
                string wahl = Dialog.leseEingabe("Händler wählen: ");

                if (wahl.ToLower() == "x") break;

                if (int.TryParse(wahl, out int verkaufIndex) && verkaufIndex >= 1 && verkaufIndex <= haendlerListe.Count)
                {
                    Haendler ausgewaehlterHaendler = haendlerListe[verkaufIndex - 1];
                    Console.Clear();
                    kunde.garageAnzeigen();
                    string fahrzeugWahl = Dialog.leseEingabe("Fahrzeug-Nr. oder 'x': ");

                    if (fahrzeugWahl.ToLower() == "x") continue;

                    if (int.TryParse(fahrzeugWahl, out int fahrzeugIndex) && fahrzeugIndex >= 1 && fahrzeugIndex <= kunde.fahrzeugGarage.Count)
                    {
                        Console.Clear();
                        VerhandlungsService.fuehreAnkaufVerhandlung(ausgewaehlterHaendler, kunde.fahrzeugGarage[fahrzeugIndex - 1], kunde);
                        Dialog.warteAufTaste("\nDrücke eine beliebige Taste, um fortzufahren...");
                        break;
                    }
                    Dialog.printHinweis("Ungültige Fahrzeugnummer.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }
    }
}