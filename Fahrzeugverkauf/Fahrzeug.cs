using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrzeugverkauf
{
    abstract class Fahrzeug
    {
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }
        public string zustand { get; set; }

        private static string[] zustandsListe = { "Sehr gut", "Gebraucht mit leichten Spuren", "Werkstattgeprüft", "Mit kleinen Kratzern", "Fast wie neu" };
        private static Random rnd = new Random();

        public Fahrzeug(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
        {
            marke = nMarke;
            modell = nModell;
            leistungInPS = nLeistungInPS;
            tankInLiter = nTankInLiter;

            // Preis variiert leicht um +/- 5% bei jedem Start für mehr Marktdynamik
            double faktor = 0.95 + rnd.NextDouble() * 0.1;
            preis = Math.Round(nPreis * faktor, 2);
            zustand = zustandsListe[rnd.Next(zustandsListe.Length)];
        }

        public virtual void fahrzeugAnzeigen()
        {
            Console.WriteLine($"Marke: {marke}, Modell: {modell}, Leistung: {leistungInPS} PS, Tank: {tankInLiter} L, Preis: {preis} EUR (Zustand: {zustand})");
        }
    }

    class Auto : Fahrzeug
    {
        public int anzahlSitzplaetze { get; set; }

        public Auto(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis, int nAnzahlSitzplaetze)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
            anzahlSitzplaetze = nAnzahlSitzplaetze;
        }

        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine($" -> Fahrzeugtyp: Auto, Sitzplätze: {anzahlSitzplaetze}");
        }
    }

    class Motorrad : Fahrzeug
    {
        public Motorrad(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
        }

        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine(" -> Fahrzeugtyp: Motorrad");
        }
    }
}
