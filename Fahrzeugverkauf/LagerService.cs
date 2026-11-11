using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Fahrzeugverkauf
{
    public class HaendlerDto
    {
        public string haendlerName { get; set; }
    }

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

    public static class LagerService
    {
        private const int GeduldMinimum = 2;
        private const int GeduldMaximum = 6;

        public static List<Haendler> ladeLagerDaten()
        {
            List<Haendler> haendlerListe = new List<Haendler>();
            Random rnd = new Random();

            string dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                Directory.CreateDirectory(dataDirectory);
            }

            string haendlerDatei = Path.Combine(dataDirectory, "haendler.json");
            string autoDatei = Path.Combine(dataDirectory, "auto.json");

            try
            {
                if (File.Exists(haendlerDatei))
                {
                    string haendlerJson = File.ReadAllText(haendlerDatei);
                    var serializer = new JavaScriptSerializer();
                    var haendlerDtos = serializer.Deserialize<List<HaendlerDto>>(haendlerJson);

                    foreach (var haendlerDto in haendlerDtos)
                    {
                        int zufallsGeduld = rnd.Next(GeduldMinimum, GeduldMaximum);
                        haendlerListe.Add(new Haendler(haendlerDto.haendlerName, zufallsGeduld));
                    }
                }
                else
                {
                    Dialog.zeigeJsonFehler("haendler.json", haendlerDatei);
                    haendlerListe.Add(new Haendler("Autohaus Müller", rnd.Next(GeduldMinimum, GeduldMaximum)));
                    haendlerListe.Add(new Haendler("Gebrauchtwagen-Paradies", rnd.Next(GeduldMinimum, GeduldMaximum)));
                }

                if (File.Exists(autoDatei))
                {
                    string autoJson = File.ReadAllText(autoDatei);
                    var serializer = new JavaScriptSerializer();
                    var fahrzeugDtos = serializer.Deserialize<List<FahrzeugDto>>(autoJson);

                    foreach (var fahrzeugDto in fahrzeugDtos)
                    {
                        Fahrzeug neuesFahrzeug;
                        if (fahrzeugDto.typ.Equals("Auto", StringComparison.OrdinalIgnoreCase))
                        {
                            neuesFahrzeug = new Auto(fahrzeugDto.marke, fahrzeugDto.modell, fahrzeugDto.leistungInPS, fahrzeugDto.tankInLiter, fahrzeugDto.preis, fahrzeugDto.anzahlSitzplaetze);
                        }
                        else
                        {
                            neuesFahrzeug = new Motorrad(fahrzeugDto.marke, fahrzeugDto.modell, fahrzeugDto.leistungInPS, fahrzeugDto.tankInLiter, fahrzeugDto.preis);
                        }

                        var zielHaendler = haendlerListe.Find(haendler => haendler.haendlerName.Equals(fahrzeugDto.haendlerName, StringComparison.OrdinalIgnoreCase));
                        if (zielHaendler != null)
                        {
                            zielHaendler.fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                        else if (haendlerListe.Count > 0)
                        {
                            haendlerListe[0].fahrzeugHinzufuegen(neuesFahrzeug);
                        }
                    }
                }
                else
                {
                    Dialog.zeigeJsonFehler("auto.json", autoDatei);
                }
            }
            catch (Exception ex)
            {
                Dialog.zeigeAllgemeinerFehler(ex.Message);
            }

            return haendlerListe;
        }
    }
}
