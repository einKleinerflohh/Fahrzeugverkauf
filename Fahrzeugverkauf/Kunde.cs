using System;
using System.Collections.Generic;

namespace Fahrzeugverkauf
{
    public class Kunde
    {
        public string vorname { get; set; }
        public string name { get; set; }
        public double budget { get; set; }

        public List<Fahrzeug> fahrzeugGarage { get; set; }

        private static readonly Random rnd = new Random();

        private const double BudgetVariationsMinimum = 0.9;
        private const double BudgetVariationsSpanne = 0.2;

        private const string StandardFahrzeugMarke = "Opel";
        private const string StandardFahrzeugModell = "Corsa";
        private const double StandardFahrzeugLeistung = 75;
        private const double StandardFahrzeugTank = 45.0;
        private const double StandardFahrzeugPreis = 3500.0;
        private const int StandardFahrzeugSitzplaetze = 3;

        public Kunde(string nVorname, string nName, double nBudget)
        {
            vorname = nVorname;
            name = nName;

            double budgetFaktor = BudgetVariationsMinimum + rnd.NextDouble() * BudgetVariationsSpanne;
            budget = Math.Round(nBudget * budgetFaktor, 2);

            fahrzeugGarage = new List<Fahrzeug>
            {
                new Auto(StandardFahrzeugMarke, StandardFahrzeugModell, StandardFahrzeugLeistung, StandardFahrzeugTank, StandardFahrzeugPreis, StandardFahrzeugSitzplaetze)
            };
        }

        public void garageAnzeigen()
        {
            Dialog.zeigeGarageTitel(vorname, name, budget);
            if (fahrzeugGarage.Count == 0)
            {
                Dialog.zeigeGarageLeer();
                return;
            }

            for (int i = 0; i < fahrzeugGarage.Count; i++)
            {
                Dialog.zeigeListeneintrag(i + 1);
                fahrzeugGarage[i].fahrzeugAnzeigen();
            }
            Dialog.zeigeGarageTrennlinie();
        }
    }
}
