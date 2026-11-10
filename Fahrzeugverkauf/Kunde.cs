using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Klasse "Kunde" repräsentiert den Käufer und Verkäufer mit persönlichen Daten, Budget und einer eigenen Fahrzeuggarage
    class Kunde
    {
        public string vorname { get; set; }
        public string name { get; set; }
        public double budget { get; set; }

        // Liste zur Verwaltung der Fahrzeuge, die sich aktuell im Besitz des Kunden befinden (Garage)
        public List<Fahrzeug> fahrzeugGarage { get; set; }

        private static Random rnd = new Random();

        // Konstruktor zur Initialisierung der Kundenattribute und der Startgarage
        public Kunde(string nVorname, string nName, double nBudget)
        {
            vorname = nVorname;
            name = nName;

            // Startbudget variiert leicht um +/- 10% für mehr Unvorhersehbarkeit
            double budgetFaktor = 0.9 + rnd.NextDouble() * 0.2;
            budget = Math.Round(nBudget * budgetFaktor, 2);

            fahrzeugGarage = new List<Fahrzeug>();

            // Dem Kunden wird zum Start ein gebrauchtes Fahrzeug in die Garage gestellt (nutzt die neuen Zustände der Basisklasse)
            fahrzeugGarage.Add(new Auto("Opel", "Corsa", 75, 45.0, 3500.0, 3));
        }

        // Gibt die Fahrzeuge in der Garage des Kunden formatiert auf der Konsole aus
        public void garageAnzeigen()
        {
            Console.WriteLine($"\n--- Fahrzeuggarage von {vorname} {name} (Budget: {budget} EUR) ---");
            if (fahrzeugGarage.Count == 0)
            {
                Console.WriteLine("Deine Garage ist aktuell leer.");
                Console.WriteLine("--------------------------------------");
                return;
            }

            for (int i = 0; i < fahrzeugGarage.Count; i++)
            {
                Console.Write($"[{i + 1}] ");
                fahrzeugGarage[i].fahrzeugAnzeigen();
            }
            Console.WriteLine("--------------------------------------\n");
        }
    }
}

