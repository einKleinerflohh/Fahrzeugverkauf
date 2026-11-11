using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Fahrzeugverkauf
{
    // DTO (Data Transfer Object) zur Deserialisierung von Händlerdaten aus JSON
    public class HaendlerDto
    {
        public string haendlerName { get; set; }
    }

    // DTO zur Deserialisierung von Fahrzeugdaten aus JSON
    public class FahrzeugDto
    {
        public string typ { get; set; }
        public string haendlerName { get; set; }
        public string marke { get; set; }
        public string modell { get; set; }
        public double leistungInPS { get; set; }
        public double tankInLiter { get; set; }
        public double preis { get; set; }
        public int anzahlSitzplaetze { get; set; }
    }

    // Service zum Laden und Initialisieren der Händler- und Bestandsdaten aus JSON-Dateien
    public static class LagerService
    {
        public static List<Haendler> ladeLagerDaten()
        {
            List<Haendler> haendlerListe = new List<Haendler>();
            Random rnd = new Random();

            // Zielpfad 'Data' im Anwendungsverzeichnis sicherstellen
            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string haendlerDatei = Path.Combine(dataDirectory, "haendler.json");
            string autoDatei = Path.Combine(dataDirectory, "auto.json");

            try
            {
                // 1. Händler aus 'haendler.json' einlesen
                if (File.Exists(haendlerDatei))
                {
                    string haendlerJson = File.ReadAllText(haendlerDatei);
                    var serializer = new JavaScriptSerializer();
                    var haendlerDtos = serializer.Deserialize<List<HaendlerDto>>(haendlerJson);

                    foreach (var hDto in haendlerDtos)
                    {
                        int zufallsGeduld = rnd.Next(2, 6);
                        haendlerListe.Add(new Haendler(hDto.haendlerName, zufallsGeduld));
                    }
                }
                else
                {
                    // Fallback, wenn Händler-JSON nicht vorhanden ist
                    Console.WriteLine($"[Hinweis] '{haendlerDatei}' nicht gefunden. Standardwerte werden genutzt.");
                    haendlerListe.Add(new Haendler("Autohaus Müller", rnd.Next(2, 6)));
                    haendlerListe.Add(new Haendler("Gebrauchtwagen-Paradies", rnd.Next(2, 6)));
                }

                // 2. Fahrzeuge aus 'auto.json' einlesen und den jeweiligen Händlern zuweisen
                if (File.Exists(autoDatei))
                {
                    string autoJson = File.ReadAllText(autoDatei);
                    var serializer = new JavaScriptSerializer();
                    var fahrzeugDtos = serializer.Deserialize<List<FahrzeugDto>>(autoJson);

                    foreach (var fDto in fahrzeugDtos)
                    {
                        Fahrzeug neuesFahrzeug;
                        if (fDto.typ.Equals("Auto", StringComparison.OrdinalIgnoreCase))
                        {
                            neuesFahrzeug = new Auto(fDto.marke, fDto.modell, fDto.leistungInPS, fDto.tankInLiter, fDto.preis, fDto.anzahlSitzplaetze);
                        }
                        else
                        {
                            neuesFahrzeug = new Motorrad(fDto.marke, fDto.modell, fDto.leistungInPS, fDto.tankInLiter, fDto.preis);
                        }

                        // Fahrzeug dem zugewiesenen Händler hinzufügen
                        var zielHaendler = haendlerListe.Find(h => h.haendlerName.Equals(fDto.haendlerName, StringComparison.OrdinalIgnoreCase));
                        if (zielHaendler != null)
                        {
                            zielHaendler.fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                        else if (haendlerListe.Count > 0)
                        {
                            // Erstbesten Händler als Rückfalloption nehmen
                            haendlerListe[0].fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"[Hinweis] '{autoDatei}' nicht gefunden.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Fehler beim Laden der JSON-Dateien]: {ex.Message}");
            }

            return haendlerListe;
        }
    }
}
