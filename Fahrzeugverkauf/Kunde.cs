using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    // Repräsentiert den Kunden mit Personalien, Budget und eigener Garage
    public class Kunde
    {
        public string vorname { get; set; }
        public string name { get; set; }
        public double budget { get; set; }

        // Liste der Fahrzeuge in Kundenbesitz
        public List<Fahrzeug> fahrzeugGarage { get; set; }

        private static readonly Random rnd = new Random();

        // Konstruktor: Initialisiert den Kunden und stellt ein Erstfahrzeug bereit
        public Kunde(string nVorname, string nName, double nBudget)
        {
            vorname = nVorname;
            name = nName;

            // Budget leicht variieren (+/- 10%)
            double budgetFaktor = 0.9 + rnd.NextDouble() * 0.2;
            budget = Math.Round(nBudget * budgetFaktor, 2);

            // Erstausstattung mit einem Gebrauchtwagen (Opel Corsa)
            fahrzeugGarage = new List<Fahrzeug>
            {
                new Auto("Opel", "Corsa", 75, 45.0, 3500.0, 3)
            };
        }

        // Gibt die Fahrzeuge in der eigenen Garage sowie das aktuelle Budget auf der Konsole aus
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
