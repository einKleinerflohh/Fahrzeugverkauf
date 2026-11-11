using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Einstiegspunkt des Programms
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Fahrzeugverkauf Simulator";
            Console.WriteLine("==============================================");
            Console.WriteLine("   WILLKOMMEN ZUM FAHRZEUGVERKAUF-SIMULATOR   ");
            Console.WriteLine("==============================================\n");

            // 1. Kundendaten erfassen
            string vorname = Dialog.leseEingabe("Bitte gib deinen Vornamen ein: ");
            string nachname = Dialog.leseEingabe("Bitte gib deinen Nachnamen ein: ");

            double budget = 0;
            while (budget <= 0)
            {
                string budgetInput = Dialog.leseEingabe("Bitte gib dein Startbudget (in EUR) ein: ");
                if (!double.TryParse(budgetInput, out budget) || budget <= 0)
                {
                    Dialog.printNachricht("Ungültiges Budget. Bitte gib eine positive Zahl ein.");
                }
            }

            // Kunde-Objekt instanziieren
            Kunde kunde = new Kunde(vorname, nachname, budget);

            // 2. Daten (Händler & Fahrzeuge) über LagerService aus JSON laden
            List<Haendler> haendlerListe = LagerService.ladeLagerDaten();

            // 3. Menü starten und Anwendung ausführen
            Menu menu = new Menu(haendlerListe, kunde);
            menu.starteHauptmenue();
        }
    }
}