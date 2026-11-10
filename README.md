Fahrzeugverkauf Simulator

Eine interaktive C#-Konsolenanwendung zur Simulation eines Fahrzeugmarkts. Das Projekt demonstriert den Einsatz von objektorientierter Programmierung (OOP) in .NET Framework mit Funktionen wie Fahrzeugkauf, Fahrzeugverkauf (Ankauf-System), dynamischen Verhandlungsstrategien und einer Garage-Verwaltung.

Hauptfunktionen

Objektorientierte Datenstruktur:

Abstrakte Basisklasse Fahrzeug mit den Unterklassen Auto (inkl. Sitzplätzen) und Motorrad.

Modular aufgebautes Kunden- und Händlersystem.

Interaktives Verhandlungssystem (Kauf):

Verschiedene Verhandlungsstrategien: Gebote abgeben, Argumente nutzen (Zustand bemängeln, Konkurrenzdruck) oder den Barzahlungs-Joker wählen.

Dynamische Händler-Geduld: Geduld sinkt bei unfairen Geboten. Bei unter 70 % des Preises greift die "Frechheitsgrenze" (Abbruch).

Ankauf-System (Verkauf eigener Fahrzeuge):

Verkaufe Fahrzeuge aus deiner eigenen Garage an Händler.

Händler kalkulieren eigene Ankaufspreise und lassen sich mit passenden Argumenten (z. B. Scheckheftpflege, neuer TÜV) hochhandeln.

Mehrere Händler zur Auswahl:

Verschiedene Händler mit individuellem Sortiment und unterschiedlichen Geduldsstufen (z. B. Autohaus Müller, Gebrauchtwagen-Paradies, Zweirad & Auto Schmidt).

Garagen- & Budgetverwaltung:

Guthaben wird bei Käufen abgezogen und bei Verkäufen gutgeschrieben.

Startausstattung des Spielers mit einem Erstfahrzeug (Opel Corsa).

Technologien & Anforderungen

Sprache: C#

Framework: .NET Framework 4.7.2

Projekttyp: Konsolenanwendung (Console Application)

Entwicklungsumgebung: Visual Studio 2017 / 2019 / 2022

Projektstruktur

Fahrzeugverkauf/
│
├── App.config              # Konfigurationsdatei (.NET Framework Version)
├── Fahrzeug.cs             # Basisklasse 'Fahrzeug' sowie 'Auto' und 'Motorrad'
├── Haendler.cs             # Logik für Händler, Bestand & Verhandlungsführung
├── Kunde.cs                # Kundendaten, Budget & Garagenverwaltung
├── Program.cs              # Hauptmenü, Menüführung & Anwendungseinstieg
├── Fahrzeugverkauf.csproj  # Visual Studio Projektdatei
├── Changelog.txt           # Detaillierte Versionshistorie (V1.0.0 bis V2.2.0)
└── Hinweis.txt             # Hinweis auf fiktionale Daten


Installation & Start

Repository klonen oder herunterladen:
Entpacke das Projekt in einen Ordner deiner Wahl.

In Visual Studio öffnen:
Doppelklicke auf die Datei Fahrzeugverkauf.csproj oder öffne das Projekt über Visual Studio (Datei ➔ Öffnen ➔ Projekt/Projektmappe).

Projekt bauen und ausführen:
Drücke F5 oder klicke auf Starten, um die Anwendung zu kompilieren und in der Konsole auszuführen.

Bedienung

Start: Gib zu Beginn deinen Vornamen, Nachnamen und dein Startbudget ein.

Hauptmenü:

[1] Alle Händler & Bestände: Verschaffe dir einen Überblick über alle verfuegbaren Autos und Motorräder.

[2] Fahrzeug kaufen: Wähle einen Händler und ein Fahrzeug aus, um in die Preisverhandlung einzusteigen.

[3] Garage anzeigen: Sieh dir deine aktuell besessenen Fahrzeuge an.

[4] Fahrzeug verkaufen: Biete ein Fahrzeug aus deiner Garage einem Händler zum Ankauf an und verhandle den Verkaufspreis.

[x] Beenden: Beendet das Programm.

Versionshistorie (Auszug)

V2.2.0: Ankauf-System (Fahrzeugverkauf durch den Kunden an Händler) und Garagen-Feature integriert.

V2.1.0: Rundenbasiertes Verhandlungssystem mit Strategien (Argumente, Barzahlung) & Händler-Geduldssystem.

V2.0.0: Unterstützung für mehrere Händler und erweitertes Fahrzeugangebot.

V1.1.0: Verhandlungs-Logik und Angebotssystem eingeführt.

V1.0.0: Erstveröffentlichung mit Grundstruktur (Klassen, Bestand & Konsolenmenü).

Siehe Changelog.txt für die vollständige Änderungshistorie.

ℹ️ Hinweis

Dieses Programm ist vollständig fiktional. Alle Namen, Adressen, Fahrzeuge und Beträge dienen lediglich Demonstrations- und Testzwecken.
