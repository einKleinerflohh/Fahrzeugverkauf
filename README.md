Fahrzeugverkauf (Console Application)

Eine C#-Konsolenanwendung zur Simulation eines Fahrzeugverkaufs bei einem oder mehreren Autohändlern. Das Projekt demonstriert grundlegende Prinzipien der objektorientierten Programmierung (OOP) wie Vererbung, Kapselung und Datenverwaltung.

Projektbeschreibung

Das Programm simuliert den Verkaufsprozess von Fahrzeugen zwischen Händlern und Kunden.
Kunden können den Bestand der Händler einsehen, Preisvorschläge für gewünschte Fahrzeuge abgeben und Käufe tätigen. Der Händler entscheidet über die Annahme von Angeboten, während das System sicherstellt, dass das Kundenbudget ausreicht und der Fahrzeugbestand entsprechend aktualisiert wird.

Klassen & Objektstruktur

Fahrzeuge (Fahrzeug)

Basisklasse für alle Fahrzeuge im System.

Gemeinsame Eigenschaften:

Marke (z. B. Audi, BMW, Aprilia, Honda)

Leistung in PS

Tankgröße in Litern

Preis (in €)

Unterklassen:

Auto

Erbt von Fahrzeug

Zusätzliche Eigenschaft: Anzahl Sitzplätze


Motorrad

Erbt von Fahrzeug


Akteure

Händler

Verwaltet den eigenen Fahrzeugbestand (Hinzufügen, Entfernen, Auflisten).

Prüft und akzeptiert/lehnt Preisvorschläge von Kunden ab.

Kunde

Besitzt ein festgelegtes Budget.

Kann Fahrzeuge durchsuchen und Kaufangebote abgeben.

Features & Funktionalitäten

Grundfunktionen

Fahrzeugverwaltung: Anlegen und Anzeigen von Autos und Motorrädern mit spezifischen Eigenschaften.

Kundenverwaltung: Erstellen von Kunden mit individuellem Budget.

Kaufabwicklung:

Abgabe von Preisvorschlägen durch den Kunden.

Entscheidung durch den Händler.

Automatische Budgetprüfung (Kauf nur bei ausreichendem Budget möglich).

Aktualisierung des Budgets und Entfernung des gekauften Fahrzeugs aus dem Bestand.

Erweiterte Features

Mehrere Händler: Unterstützung verschiedener Autohäuser mit jeweils eigenem Fahrzeugbestand.

Nachverhandeln: Möglichkeit für Gegenangebote/neue Preisvorschläge (NeuerPreis), falls ein Angebot abgelehnt wird.

Weiterkaufen Loop: Interaktive Abfrage (y/n), ob der Kunde weitere Einkäufe tätigen möchte.

Fahrzeugverkauf durch Kunden: Option für Kunden, eigene Fahrzeuge wieder zu verkaufen.

Interaktive Steuerung: Benutzerfreundliche Menüführung in der Konsole.

Voraussetzungen & Installation

Anforderungen

.NET SDK (Version 6.0 oder neuer)

Visual Studio 2022 (oder Visual Studio Code mit C#-Erweiterung)

Projekt in Visual Studio öffnen

Repository klonen oder ZIP-Archiv entpacken.

Die Datei Fahrzeugverkauf.sln in Visual Studio öffnen.

Mit F5 oder Strg + F5 das Projekt bauen und starten.

🚀 NutzungBeispiel (Konsolenablauf)

=== Willkommen beim Fahrzeugverkauf ===

Verfügbare Händler:
1. Autohaus Schmidt (Audi, BMW)
2. Zweirad Meyer (Aprilia, Honda)

Wähle einen Händler: 1

[Autohaus Schmidt - Fahrzeugbestand]
1. Audi A4 | 190 PS | 54L Tank | 5 Sitze | 25.000 €
2. BMW M3  | 510 PS | 59L Tank | 5 Sitze | 60.000 €

Dein aktuelles Budget: 30.000 €
Welches Fahrzeug möchtest du kaufen? 1

Gib deinen Preisvorschlag ein (€): 23500
Händler entscheidet... 
[Händler]: Angebot akzeptiert!

Kauf erfolgreich!
Neues Budget: 6.500 €
Möchtest du weiter einkaufen? (y/n): n
