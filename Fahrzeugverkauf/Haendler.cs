using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fahrzeugverkauf
{
    // Klasse "Haendler" verwaltet den Namen, die individuelle Geduld, den Fahrzeugbestand sowie Kauf- und Ankauf-Verhandlungen
    class Haendler
    {
        public string haendlerName { get; set; }
        public int geduld { get; set; }
        private List<Fahrzeug> fahrzeugBestand;
        private Random rnd = new Random();

        public Haendler(string name, int startGeduld)
        {
            haendlerName = name;
            geduld = startGeduld;
            fahrzeugBestand = new List<Fahrzeug>();
        }

        public void fahrzeugHinzufuegen(Fahrzeug fahrzeug)
        {
            fahrzeugBestand.Add(fahrzeug);
        }

        public void fahrzeugEntfernen(Fahrzeug fahrzeug)
        {
            fahrzeugBestand.Remove(fahrzeug);
        }

        public void bestandAnzeigen()
        {
            Console.WriteLine($"\n--- Fahrzeugbestand von {haendlerName} ---");
            if (fahrzeugBestand.Count == 0)
            {
                Console.WriteLine("Der Bestand dieses Händlers ist aktuell leer.");
                Console.WriteLine("--------------------------------------");
                return;
            }

            for (int i = 0; i < fahrzeugBestand.Count; i++)
            {
                Console.Write($"[{i + 1}] ");
                fahrzeugBestand[i].fahrzeugAnzeigen();
            }
            Console.WriteLine("--------------------------------------\n");
        }

        public List<Fahrzeug> getBestand()
        {
            return fahrzeugBestand;
        }

        // Verhandlung beim Kauf eines Fahrzeugs VOM Händler (bereits vorhanden)
        public void verhandlePreis(Kunde kunde, Fahrzeug fahrzeug)
        {
            int maxVersuche = rnd.Next(3, 6);
            int aktuellerVersuch = 0;
            int aktuelleGeduld = geduld;

            double aktuellerHaendlerPreis = fahrzeug.preis;
            double schmerzgrenze = fahrzeug.preis * 0.82;
            double frechheitGrenze = fahrzeug.preis * 0.70;

            Console.Clear();
            Console.WriteLine($"\n[{haendlerName}]: \"Hallo {kunde.vorname}! Der {fahrzeug.marke} {fahrzeug.modell} kostet regulär {aktuellerHaendlerPreis} EUR. Lass uns verhandeln!\"");
            Console.WriteLine("\nDrücke eine beliebige Taste, um die Verhandlung zu starten...");
            Console.ReadKey();

            while (aktuellerVersuch < maxVersuche && aktuelleGeduld > 0)
            {
                aktuellerVersuch++;

                Console.Clear();
                Console.WriteLine($"\n--- Verhandlungsrunde {aktuellerVersuch}/{maxVersuche} | {haendlerName}-Geduld: {aktuelleGeduld}/{geduld} ---");
                Console.WriteLine($"Fahrzeug: {fahrzeug.marke} {fahrzeug.modell} (Regulärer Preis: {fahrzeug.preis} EUR)");
                Console.WriteLine($"Aktueller Angebotspreis: {aktuellerHaendlerPreis} EUR | Dein Budget: {kunde.budget} EUR");
                Console.WriteLine("[1] Konkretes Preisgebot abgeben");
                Console.WriteLine("[2] Argument anbringen (Zustand bemängeln oder Marktdruck ausüben)");
                Console.WriteLine("[3] Barzahlungs-Joker einsetzen (Sofortkauf-Rabatt erfragen)");
                Console.WriteLine("[x] Verhandlung abbrechen");
                Console.Write("Wähle deine Strategie: ");

                string wahl = Console.ReadLine()?.ToLower();

                if (wahl == "x")
                {
                    Console.Clear();
                    Console.WriteLine($"[{haendlerName}]: \"Schade! Vielleicht bis zum nächsten Mal.\"");
                    return;
                }

                if (wahl == "1")
                {
                    Console.Write("\nDein Preisvorschlag in EUR: ");
                    if (!double.TryParse(Console.ReadLine(), out double vorschlag))
                    {
                        Console.WriteLine($"[{haendlerName}]: \"Das war keine gültige Zahl. Konzentrier dich!\"");
                        aktuellerVersuch--;
                        Thread.Sleep(1500);
                        continue;
                    }

                    if (kunde.budget < vorschlag)
                    {
                        Console.WriteLine($"[{haendlerName}]: \"Du hast doch gar nicht so viel Geld dabei! Dein Budget liegt bei {kunde.budget} EUR.\"");
                        aktuellerVersuch--;
                        Thread.Sleep(1500);
                        continue;
                    }

                    if (vorschlag < frechheitGrenze)
                    {
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"{vorschlag} EUR?! Das ist eine absolute Frechheit und Beleidigung. Raus aus meinem Laden!\"");
                        return;
                    }

                    if (vorschlag >= aktuellerHaendlerPreis)
                    {
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"Hervorragend, zu dem Preis machen wir den Sack zu. Deal!\"");
                        kunde.budget -= vorschlag;
                        fahrzeugEntfernen(fahrzeug);
                        kunde.fahrzeugGarage.Add(fahrzeug); // Fahrzeug wandert in die Garage des Kunden
                        Console.WriteLine($"\n[Erfolg] Du hast den {fahrzeug.marke} {fahrzeug.modell} für {vorschlag} EUR gekauft und in deine Garage gestellt!");
                        return;
                    }

                    aktuelleGeduld--;
                    aktuellerHaendlerPreis = Math.Round((aktuellerHaendlerPreis + vorschlag) / 2.0, 2);

                    if (aktuellerHaendlerPreis < schmerzgrenze)
                    {
                        aktuellerHaendlerPreis = schmerzgrenze;
                    }

                    Console.WriteLine($"[{haendlerName}]: \"Hmm, {vorschlag} EUR ist mir etwas zu wenig. Aber ich komme dir entgegen: {aktuellerHaendlerPreis} EUR.\"");
                    Thread.Sleep(2000);
                }
                else if (wahl == "2")
                {
                    Console.Clear();
                    Console.WriteLine($"\n[{haendlerName}]: \"Was gibt es denn zu bemängeln?\"");
                    Console.WriteLine("[1] 'Der Tank ist fast leer / kleine optische Mängel.' (Kleiner Rabatt)");
                    Console.WriteLine("[2] 'Ich habe dieses Modell online viel günstiger gesehen!' (Druck ausüben)");
                    Console.Write("Wähle dein Argument: ");
                    string argWahl = Console.ReadLine();

                    if (argWahl == "1")
                    {
                        double rabatt = fahrzeug.preis * 0.03;
                        aktuellerHaendlerPreis = Math.Round(aktuellerHaendlerPreis - rabatt, 2);
                        if (aktuellerHaendlerPreis < schmerzgrenze) aktuellerHaendlerPreis = schmerzgrenze;
                        Console.WriteLine($"[{haendlerName}]: \"Da hast du recht. Ich gehe runter auf {aktuellerHaendlerPreis} EUR.\"");
                    }
                    else if (argWahl == "2")
                    {
                        if (rnd.Next(0, 2) == 1)
                        {
                            double rabatt = fahrzeug.preis * 0.06;
                            aktuellerHaendlerPreis = Math.Round(aktuellerHaendlerPreis - rabatt, 2);
                            if (aktuellerHaendlerPreis < schmerzgrenze) aktuellerHaendlerPreis = schmerzgrenze;
                            Console.WriteLine($"[{haendlerName}]: \"Verdammt, die Konkurrenz schläft nicht... Sonderpreis: {aktuellerHaendlerPreis} EUR.\"");
                        }
                        else
                        {
                            aktuelleGeduld--;
                            Console.WriteLine($"[{haendlerName}]: \"Dann geh doch zur Konkurrenz! Auf solche Lügen reibe ich nicht rein.\"");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{haendlerName}]: \"Verstehe ich nicht ganz.\"");
                        aktuellerVersuch--;
                    }
                    Thread.Sleep(2000);
                }
                else if (wahl == "3")
                {
                    double barAngebot = Math.Round(aktuellerHaendlerPreis * 0.94, 2);
                    if (barAngebot < schmerzgrenze) barAngebot = schmerzgrenze;

                    if (barAngebot <= kunde.budget)
                    {
                        Console.Write($"[{haendlerName}]: \"Bargeld sofort auf den Tisch? Für {barAngebot} EUR gehört es dir. Einverstanden? (j/n): \"");
                        string entscheidung = Console.ReadLine()?.ToLower();

                        if (entscheidung == "j")
                        {
                            Console.Clear();
                            Console.WriteLine($"[{haendlerName}]: \"Ausgemacht! Bargeld lacht.\"");
                            kunde.budget -= barAngebot;
                            fahrzeugEntfernen(fahrzeug);
                            kunde.fahrzeugGarage.Add(fahrzeug);
                            Console.WriteLine($"\n[Erfolg] Bar gekauft für {barAngebot} EUR! Restbudget: {kunde.budget} EUR.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[{haendlerName}]: \"Dein Budget reicht für die Barzahlung leider nicht aus!\"");
                    }
                    Thread.Sleep(2000);
                }
                else
                {
                    Console.WriteLine("Ungültige Option.");
                    aktuellerVersuch--;
                    Thread.Sleep(1500);
                }
            }

            Console.Clear();
            Console.WriteLine($"\n[{haendlerName}]: \"Wir drehen uns im Kreis und meine Geduld ist am Ende. Kein Geschäft!\"");
        }

        // NEU: Interaktiver Verhandlungsprozess beim Fahrzeugverkauf VOM Kunden AN den Händler (Ankauf)
        public void verhandleAnkauf(Kunde kunde, Fahrzeug fahrzeug)
        {
            int maxVersuche = rnd.Next(3, 6);
            int aktuellerVersuch = 0;
            int aktuelleGeduld = geduld;

            // Startgebot des Händlers liegt unter dem Schätzwert des Fahrzeugs (z. B. 70%), um Gewinnspanne zu sichern
            double aktuellerHaendlerGebot = Math.Round(fahrzeug.preis * 0.70, 2);
            double schmerzgrenzeHaendler = fahrzeug.preis * 0.85; // Maximaler Ankaufspreis des Händlers

            Console.Clear();
            Console.WriteLine($"\n[{haendlerName}]: \"Ah, du möchtest mir deinen {fahrzeug.marke} {fahrzeug.modell} verkaufen? Schauen wir uns das gute Stück an...\"");
            Console.WriteLine($"[{haendlerName}]: \"Für einen Weiterverkauf biete ich dir als Ersteinschätzung {aktuellerHaendlerGebot} EUR.\"");


            while (aktuellerVersuch < maxVersuche && aktuelleGeduld > 0)
            {
                aktuellerVersuch++;
                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Fahrzeug: {fahrzeug.marke} {fahrzeug.modell} (Basiswert: {fahrzeug.preis} EUR)");
                Console.WriteLine("--------------------------------------\n");
                Console.WriteLine("[1] Eigenen Wunschpreis nennen / Nachbessern");
                Console.WriteLine("[2] Argument anbringen (Scheckheftgepflegt oder top Zustand)");
                Console.WriteLine("[3] Sofort-Ankauf zum aktuellen Händler-Gebot akzeptieren");
                Console.WriteLine("[x] Verhandlung abbrechen");
                Console.Write("Wähle deine Strategie: ");

                string wahl = Console.ReadLine()?.ToLower();

                if (wahl == "x")
                {
                    Console.Clear();
                    Console.WriteLine($"[{haendlerName}]: \"Schade, dann behältst du deinen Wagen eben. Tschüss!\"");
                    return;
                }

                // OPTION 1: Eigenen Wunschpreis nennen
                if (wahl == "1")
                {
                    Console.Write("\nDein gewünschter Verkaufspreis in EUR: ");
                    if (!double.TryParse(Console.ReadLine(), out double kundenGebot))
                    {
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"Das ist keine gültige Zahl!\"");
                        aktuellerVersuch--;
                        Thread.Sleep(1500);
                        continue;
                    }

                    // Wenn der Wunschpreis völlig überzogen ist
                    if (kundenGebot > fahrzeug.preis * 1.2)
                    {
                        Console.Clear();
                        aktuelleGeduld--;
                        Console.WriteLine($"[{haendlerName}]: \"Zu dem Preis kann ich ihn niemals weiterverkaufen. Übertreib mal nicht!\"");
                        Thread.Sleep(1500);
                        continue;
                    }

                    // Wenn der Händler den Preis akzeptiert oder sich im Rahmen bewegt
                    if (kundenGebot <= schmerzgrenzeHaendler)
                    {
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"Deal! {kundenGebot} EUR ist für uns beide in Ordnung.\"");
                        kunde.budget += kundenGebot;                  // Geld gutschreiben
                        kunde.fahrzeugGarage.Remove(fahrzeug);      // Aus Kunden-Garage entfernen
                        fahrzeugHinzufuegen(fahrzeug);              // In Händler-Bestand aufnehmen
                        Console.WriteLine($"\n[Erfolg] Du hast deinen {fahrzeug.marke} {fahrzeug.modell} für {kundenGebot} EUR an {haendlerName} verkauft! Neues Budget: {kunde.budget} EUR.");
                        return;
                    }
                    else
                    {
                        Console.Clear();
                        aktuelleGeduld--;
                        aktuellerHaendlerGebot = Math.Round((aktuellerHaendlerGebot + kundenGebot) / 2.0, 2);
                        if (aktuellerHaendlerGebot > schmerzgrenzeHaendler)
                        {
                            aktuellerHaendlerGebot = schmerzgrenzeHaendler;
                        }
                        Console.WriteLine($"[{haendlerName}]: \"Das ist mir zu teuer im Ankauf. So viel kann ich nicht zahlen. Höchstens {aktuellerHaendlerGebot} EUR.\"");
                        Thread.Sleep(2000);
                    }
                }
                // OPTION 2: Argument anbringen
                else if (wahl == "2")
                {
                    Console.Clear();
                    Console.WriteLine($"\n[{haendlerName}]: \"Was macht dein Fahrzeug besonders?\"");
                    Console.WriteLine("[1] 'Scheckheftgepflegt und Garagenwagen.' (Schafft Vertrauen)");
                    Console.WriteLine("[2] 'Frisch TÜV und nagelneue Markenreifen!' (Mehrwert)");
                    Console.Write("Wähle dein Argument: ");
                    string argWahl = Console.ReadLine();

                    if (argWahl == "1" || argWahl == "2")
                    {
                        double zuschlag = fahrzeug.preis * 0.04;
                        aktuellerHaendlerGebot = Math.Round(aktuellerHaendlerGebot + zuschlag, 2);
                        if (aktuellerHaendlerGebot > schmerzgrenzeHaendler)
                        {
                            aktuellerHaendlerGebot = schmerzgrenzeHaendler;
                        }
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"Das klingt gut, das steigert den Wiederverkaufswert. Ich gehe rauf auf {aktuellerHaendlerGebot} EUR.\"");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"[{haendlerName}]: \"Das überzeugt mich jetzt nicht wirklich.\"");
                        aktuellerVersuch--;
                    }
                    Thread.Sleep(2000);
                }
                // OPTION 3: Sofort-Ankauf annehmen
                else if (wahl == "3")
                {
                    Console.Clear();
                    Console.WriteLine($"[{haendlerName}]: \"Ausgemacht! Wir nehmen den Wagen direkt für {aktuellerHaendlerGebot} EUR.\"");
                    kunde.budget += aktuellerHaendlerGebot;
                    kunde.fahrzeugGarage.Remove(fahrzeug);
                    fahrzeugHinzufuegen(fahrzeug);
                    Console.WriteLine($"\n[Erfolg] Du hast deinen {fahrzeug.marke} {fahrzeug.modell} erfolgreich für {aktuellerHaendlerGebot} EUR verkauft! Neues Budget: {kunde.budget} EUR.");
                    return;
                }
                else
                {
                    Console.WriteLine("Ungültige Option.");
                    aktuellerVersuch--;
                    Thread.Sleep(1500);
                    Console.Clear();
                }
            }

            Console.Clear();
            Console.WriteLine($"\n[{haendlerName}]: \"Wir werden uns preislich leider nicht einig. Kein Ankauf heute!\"");
        }
    }
}