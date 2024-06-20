# Diplomarbeit Custos Mitarbeiterverwaltungssystem (2023/24)

```
git clone https://github.com/Die-Spengergasse/sj23-24-5aaif-da-custos-employeemanager
```

![](screenshot_1924.png)

## Themenstellungen

| Name                     | Individuelle Themenstellung                                                                                                                    |
| ------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------- |
| Muayad Idriss (5AAIF)    | Implementierung eines Kalenders, der die österreichischen Feiertage berücksichtigt und im Web und als ICAL auf dem Smartphone verfügbar ist. |
| Arash Rahmani (5AAIF)    | Entwicklung eines Systems zur Erfassung der Aufträge, so dass sie von den einzelnen Mitarbeitern angenommen werden können.                   |
| Mihajlo Zivkovic (5AAIF) | Implementierung einer Webanwendung zum Migrieren und Bearbeiten von Mitarbeiterdaten aus externen Quellen.                                   |

Betreuer: Michael Schletz

## Inhalte

### Muayad Idriss

1. Die Kalender Komponente
   1. Was ist eine Komponente in Vue.js?
   2. Wie werden die österreichischen Feiertage im Backend ermittelt (Klasse *CalendarService*)?
   3. Wie wird der Kalender responsive angezeigt?

2. Der ICAL Export
    1. Das ICAL Format, Gründe für ICAL.
    2. Implementierung eines Controllers als Endpunkt der ICAL Subscription.
    3. Aufbereiten der Daten für das ICAL Format.
    4. Zuordnung des Users über den ICAL Request.

### Arash Rahmani

1. Das Erfassungsformular im Frontend
   1. Nutzung der Primevue Komponenten für DataTable und Dropdown.
   2. Einbindung der Komponenten in die eigene Applikation.
   3. Verwendete Styles.
   4. JavaScript Logik der Komponente.

2. Backendlogik für die Erfassung von Aufträgen
   1. Der JobsController als Endpunkt.
   2. Erforderliche Validierung der Daten.
   3. Speicherung der Jobs in der Datenbank.

### Mihajlo Zivkovic

1. Erfassung eines einzelnen Mitarbeiters.
   1. Aufbau des Formulares.
   2. Verwendete Styles.
   3. JavaScript Logik der Komponente.

2. Backendlogik für die Erfassung von Mitarbeitern
   1. Der EmployeeController als Endpunkt.
   2. Erforderliche Validierung der Daten.
   3. Speicherung der Jobs in der Datenbank.

3. Möglichkeiten eines Massenimportes von Mitarbeiterdaten
   1. Einlesen von Exceldaten in .NET.
   2. Laden der Daten in die Datenbank.

## Installation der Entwicklungsumgebung

Die Webapp benötigt **.NET 8** (Visual Studio ab 17.8) und [Node.js](https://nodejs.org/en) ab Version 20.
Für die Entwicklung wird VS Code mit folgenden Extensions empfohlen:

- [ESLint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)
- [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar)

### Starten des Servers

Führe das Skript [EmployeeManager/start_server.sh](EmployeeManager/start_server.sh) in der git bash aus.
Es wird ein Build der SPA durchgeführt und die Webapi wird gestartet.
Es ist jedes Login gültig.
