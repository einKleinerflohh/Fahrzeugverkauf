using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrzeugverkauf
{
    // Basisklasse "Fahrzeug" (abstrakt, da sie als allgemeine Vorlage dient und nicht direkt instanziiert werden soll)
    abstract class Fahrzeug
    {
        // Öffentliche Eigenschaften eines Fahrzeugs (können von außen gelesen und beschrieben werden)
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }

        // Konstruktor der Klasse "Fahrzeug" zur Initialisierung der Grundattribute beim Erstellen eines Objekts
        public Fahrzeug(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
        {
            marke = nMarke;
            modell = nModell;
            leistungInPS = nLeistungInPS;
            tankInLiter = nTankInLiter;
            preis = nPreis;
        }

        // Virtuelle Methode zum Anzeigen der Fahrzeugdetails (kann von Unterklassen überschrieben werden)
        public virtual void fahrzeugAnzeigen()
        {
            Console.WriteLine($"Marke: {marke}, Modell: {modell}, Leistung: {leistungInPS} PS, Tankgröße: {tankInLiter} L, Preis: {preis} EUR");
        }
    }

    // Unterklasse "Auto", erbt von der Basisklasse "Fahrzeug"
    class Auto : Fahrzeug
    {
        // Zusätzliche Eigenschaft, die nur Autos besitzen
        public int anzahlSitzplaetze { get; set; }

        // Konstruktor der Klasse "Auto", nutzt ": base()" um die geerbten Eigenschaften an die Basisklasse zu übergeben
        public Auto(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis, int nAnzahlSitzplaetze)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
            anzahlSitzplaetze = nAnzahlSitzplaetze;
        }

        // Überschriebene Methode zum Anzeigen der Auto-spezifischen Details (ruft zusätzlich die Basis-Methode auf)
        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine($" -> Fahrzeugtyp: Auto, Sitzplätze: {anzahlSitzplaetze}");
        }
    }

    // Unterklasse "Motorrad", erbt von der Basisklasse "Fahrzeug"
    class Motorrad : Fahrzeug
    {
        // Konstruktor der Klasse "Motorrad" (nutzt direkt den Konstruktor der Basisklasse)
        public Motorrad(string nMarke, string nModell, double nLeistungInPS, double nTankInLiter, double nPreis)
            : base(nMarke, nModell, nLeistungInPS, nTankInLiter, nPreis)
        {
        }

        // Überschriebene Methode zum Anzeigen der Motorrad-spezifischen Details
        public override void fahrzeugAnzeigen()
        {
            base.fahrzeugAnzeigen();
            Console.WriteLine(" -> Fahrzeugtyp: Motorrad");
        }
    }
}
