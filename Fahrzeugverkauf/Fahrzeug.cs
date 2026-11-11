using System;

namespace Fahrzeugverkauf
{
    public abstract class Fahrzeug
    {
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }
        public string zustand { get; set; }

        private const double PreisVariationsMinimumFaktor = 0.95;
        private const double PreisVariationsBereich = 0.1;

        private static readonly string[] zustandsListe = {
            "Sehr gut",
            "Gebraucht mit leichten Spuren",
            "Werkstattgeprüft",
            "Mit kleinen Kratzern",
            "Fast wie neu"
        };

        private static readonly Random rnd = new Random();

        public Fahrzeug(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
        {
            marke = nMarke;
            modell = nModell;
            leistungInPS = nLeistungInPS;
            tankInLiter = nTankInLiter;

            double faktor = PreisVariationsMinimumFaktor + rnd.NextDouble() * PreisVariationsBereich;
            preis = Math.Round(nPreis * faktor, 2);

            zustand = zustandsListe[rnd.Next(zustandsListe.Length)];
        }

        public virtual void fahrzeugAnzeigen()
        {
            Dialog.zeigeFahrzeugDetails(marke, modell, leistungInPS, tankInLiter, preis, zustand);
            Console.WriteLine();
        }
    }

    public class Auto : Fahrzeug
    {
        public int anzahlSitzplaetze { get; set; }

        public Auto(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis, int nAnzahlSitzplaetze)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
            anzahlSitzplaetze = nAnzahlSitzplaetze;
        }

        public override void fahrzeugAnzeigen()
        {
            Dialog.zeigeFahrzeugDetails(marke, modell, leistungInPS, tankInLiter, preis, zustand);
            Dialog.zeigeAutoDetails(anzahlSitzplaetze);
        }
    }

    public class Motorrad : Fahrzeug
    {
        public Motorrad(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
        }

        public override void fahrzeugAnzeigen()
        {
            Dialog.zeigeFahrzeugDetails(marke, modell, leistungInPS, tankInLiter, preis, zustand);
            Dialog.zeigeMotorradDetails();
        }
    }
}
