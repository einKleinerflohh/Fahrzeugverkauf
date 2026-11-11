using System;

namespace Fahrzeugverkauf
{
    // Abstrakte Basisklasse für alle Fahrzeuge im System.
    // Kann nicht direkt instanziiert werden, dient als Vorlage für konkrete Fahrzeugtypen.
    public abstract class Fahrzeug
    {
        // Öffentliche Eigenschaften für Fahrzeugdaten
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }
        public string zustand { get; set; }

        // Vordefinierte Zustände für die zufällige Zuweisung beim Erstellen
        private static readonly string[] zustandsListe = {
            "Sehr gut",
            "Gebraucht mit leichten Spuren",
            "Werkstattgeprüft",
            "Mit kleinen Kratzern",
            "Fast wie neu"
        };

        // Zufallsgenerator für Preisvariation und Zustand
        private static readonly Random rnd = new Random();

        // Konstruktor: Initialisiert die Basisdaten und berechnet dynamisch Preis & Zustand
        public Fahrzeug(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
        {
            marke = nMarke;
            modell = nModell;
            leistungInPS = nLeistungInPS;
            tankInLiter = nTankInLiter;

            // Preis variiert zufällig um +/- 5% vom Basispreis
            double faktor = 0.95 + rnd.NextDouble() * 0.1;
            preis = Math.Round(nPreis * faktor, 2);

            // Zufälliger Zustand aus der Vorlage auswählen
            zustand = zustandsListe[rnd.Next(zustandsListe.Length)];
        }

        // Virtuelle Methode zur Konsolenausgabe der Fahrzeugdaten (kann von Unterklassen erweitert werden)
        public virtual void fahrzeugAnzeigen()
        {
            Console.WriteLine($"Marke: {marke}, Modell: {modell}, Leistung: {leistungInPS} PS, Tank: {tankInLiter} L, Preis: {preis} EUR (Zustand: {zustand})");
        }
    }

    // Konkrete Unterklasse für Autos
    public class Auto : Fahrzeug
    {
        // Zusätzliche spezifische Eigenschaft für Autos
        public int anzahlSitzplaetze { get; set; }

        // Konstruktor leitet Parameter an die Basisklasse weiter
        public Auto(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis, int nAnzahlSitzplaetze)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
            anzahlSitzplaetze = nAnzahlSitzplaetze;
        }

        // Überschreibt die Anzeige der Basisklasse um Auto-spezifische Daten
        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine($" -> Fahrzeugtyp: Auto, Sitzplätze: {anzahlSitzplaetze}");
        }
    }

    // Konkrete Unterklasse für Motorräder
    public class Motorrad : Fahrzeug
    {
        public Motorrad(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
        }

        // Überschreibt die Anzeige der Basisklasse um Motorrad-spezifische Daten
        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine(" -> Fahrzeugtyp: Motorrad");
        }
    }
}
