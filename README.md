Fahrzeugverkauf Simulator

Eine interaktive C#-Konsolenanwendung zur Simulation eines Fahrzeugmarkts. Das Projekt demonstriert den Einsatz von objektorientierter Programmierung (OOP) in .NET Framework mit Funktionen wie JSON-Datenanbindung, Fahrzeugkauf, Fahrzeugverkauf (Ankauf-System), dynamischen Verhandlungsstrategien und einer Garage-Verwaltung.

Hauptfunktionen

Objektorientierte Datenstruktur:
- Abstrakte Basisklasse Fahrzeug mit den Unterklassen Auto (inkl. Sitzplätzen) und Motorrad sowie automatischer Preisvariation und Zustandsvergabe.
- Modular aufgebautes Kunden- und Händlersystem.

JSON-Datenanbindung:
- Automatisches Laden von Händler- ('haendler.json') und Fahrzeugdaten ('auto.json') aus einem separaten 'Data'-Verzeichnis.
- Fallback-Mechanismus mit Standardwerten bei fehlenden Konfigurationsdateien.

Interaktives Verhandlungssystem (Kauf):
- Verschiedene Verhandlungsstrategien: Gebote abgeben, Argumente nutzen (Zustand bemängeln, Konkurrenzdruck) oder den Barzahlungs-Joker wählen.
- Dynamische Händler-Geduld & Stimmungen: Geduld sinkt bei unfairen Geboten. Die aktuelle Stimmung des Händlers beeinflusst die Verhandlungsbereitschaft und Schmerzgrenzen. Bei unter 70 % des Preises greift die "Frechheitsgrenze" (Abbruch).

Ankauf-System (Verkauf eigener Fahrzeuge):
- Verkaufe Fahrzeuge aus deiner eigenen Garage an Händler.
- Händler kalkulieren eigene Ankaufspreise und lassen sich mit passenden Argumenten hochhandeln.

Modularer Aufbau (Menu.cs):
- Klare Trennung der Konsolen- und Menülogik für eine strukturierte Navigation zwischen Hauptmenü, Kauf und Verkauf.

Mehrere Händler zur Auswahl:
- Verschiedene Händler mit individuellem Sortiment und unterschiedlichen Geduldsstufen (z. B. Autohaus Müller, Gebrauchtwagen-Paradies, Zweirad & Auto Schmidt).

Garagen- & Budgetverwaltung:
- Guthaben wird bei Käufen abgezogen und bei Verkäufen gutgeschrieben.
- Startausstattung des Spielers mit einem Erstfahrzeug (Opel Corsa).

Technologien & Anforderungen

Sprache: C#
Framework: .NET Framework 4.7.2
Projekttyp: Konsolenanwendung (Console Application)
Entwicklungsumgebung: Visual Studio 2017 / 2019 / 2022

Projektstruktur

Fahrzeugverkauf/
│
├── Data/                   # Verzeichnis für JSON-Daten (haendler.json, auto.json)
├── App.config              # Konfigurationsdatei (.NET Framework Version)
├── Fahrzeug.cs             # Basisklasse 'Fahrzeug' sowie 'Auto' und 'Motorrad'
├── Haendler.cs             # Logik für Händler, Bestand, Stimmungen & Verhandlungsführung
├── Kunde.cs                # Kundendaten, Budget & Garagenverwaltung
├── Menu.cs                 # Modulare Menü- und Navigationssteuerung
├── Program.cs              # Anwendungseinstieg, JSON-Deserialisierung & Konsolen-Hilfsmethoden
├── Fahrzeugverkauf.csproj  # Visual Studio Projektdatei
├── Changelog.txt           # Detaillierte Versionshistorie (V1.0.0 bis V3.0.0)
└── Hinweis.txt             # Hinweis auf fiktionale Daten

Installation & Start

Repository klonen oder herunterladen:
Entpacke das Projekt in einen Ordner deiner Wahl.

JSON-Daten vorbereiten:
Stelle sicher, dass sich im Anwendungsverzeichnis der Ordner `Data` mit den entsprechenden `.json`-Dateien befindet (oder lass das Programm die Standardwerte greifen).

In Visual Studio öffnen:
Doppelklicke auf die Datei Fahrzeugverkauf.csproj oder öffne das Projekt über Visual Studio (Datei ➔ Öffnen ➔ Projekt/Projektmappe).

Projekt bauen und ausführen:
Drücke F5 oder klicke auf Starten, um die Anwendung zu kompilieren und in der Konsole auszuführen.

Bedienung

Hauptmenü:
[1] Alle Händler & Bestände anzeigen: Verschaffe dir einen Überblick über alle verfügbaren Autos und Motorräder.
[2] Fahrzeug bei einem Händler kaufen (Verhandeln): Wähle einen Händler und ein Fahrzeug aus, um in die Preisverhandlung einzusteigen.
[3] Eigene Garage anzeigen: Sieh dir deine aktuell besessenen Fahrzeuge an.
[4] Fahrzeug an einen Händler verkaufen: Biete ein Fahrzeug aus deiner Garage einem Händler zum Ankauf an und verhandle den Verkaufspreis.
[x] Programm beenden: Beendet die Anwendung.

Versionshistorie (Auszug)

V3.0.0: JSON-Datenanbindung, Auslagerung der Menülogik in 'Menu.cs' und Einführung von Händler-Stimmungen sowie zentralem Verhandlungs-Enum integriert.
V2.2.0: Ankauf-System (Fahrzeugverkauf durch den Kunden an Händler) und Garagen-Feature integriert.
V2.1.0: Rundenbasiertes Verhandlungssystem mit Strategien (Argumente, Barzahlung) & Händler-Geduldssystem.
V2.0.0: Unterstützung für mehrere Händler und erweitertes Fahrzeugangebot.
V1.1.0: Verhandlungs-Logik und Angebotssystem eingeführt.
V1.0.0: Erstveröffentlichung mit Grundstruktur (Klassen, Bestand & Konsolenmenü).

Siehe Changelog.txt für die vollständige Änderungshistorie.

Hinweis:

Dieses Programm ist vollständig fiktional. Alle Namen, Adressen, Fahrzeuge und Beträge dienen lediglich Demonstrations- und Testzwecken.