# Baobab Ridge Wildlife Rehabilitation Records System

PRG2782 group project, Belgium Campus, 2026. A C# Windows Forms application
for recording wildlife rehabilitation and producing a text summary.

## Team

| Member | Name | Student number | Feature |
| --- | --- | --- | --- |
| 1 | Alexander Meyer | 604082 | Add Animal, validation and shared classification |
| 2 | Golokile Phomolo Seleke | 605189 | View Animals and file loading |
| 3 | Willem Abraham Jacobus Kruger | 600221 | Search, update and delete |
| 4 | Dean Martin Hendrikz | 603341 | Summary report, starter data, README and GitHub management |

## Build and run

Use Windows with the .NET 8 SDK (or a newer SDK with .NET 8 desktop targeting support).
For Visual Studio, install the .NET desktop development workload, open
`BaobabRidge.sln` as a solution, restore dependencies, build, and press F5.
Double-click MainForm.cs in Solution Explorer for the visual form designer.
Use Ctrl+Alt+X for Toolbox and F4 for Properties.
Opening an individual code file or using Open Folder does not
open the visual designer.

From a terminal in a fresh clone:

```powershell
git clone https://github.com/alexxmeyerr/baobab-ridge-wildlife.git
cd baobab-ridge-wildlife
dotnet restore BaobabRidge.csproj
dotnet build BaobabRidge.csproj -c Release
dotnet run --project BaobabRidge.csproj -c Release
```

No database, connection string, machine-specific path or third-party package is needed.
The project copies `Data/animals.txt` to `bin/Release/net8.0-windows/animals.txt`
(or the corresponding Debug output folder). The running application always uses
`animals.txt` and `summary.txt` beside its executable.

## Features

- **Add:** enter an ID, name, species, age and recovery score. IDs are trimmed,
  uppercased and checked against the current file for duplicates before appending.
  Name and species are trimmed, required, 1–30 characters and cannot contain `|`
  or line breaks. Age and score must be whole numbers from 0 to 100.
- **View:** startup loads valid records into a read-only grid in file order.
  Missing `animals.txt` is created empty. Malformed lines are skipped and counted
  in one load message below the form. Sorting is disabled to preserve file order.
- **Update:** click a row or search by ID. Edit Mode locks the ID, disables Add,
  and enables Save Changes. Saving revalidates and reclassifies the animal,
  replaces only that record, refreshes the grid, and clears the inputs.
  A failed search preserves typed input. Clear cancels editing.
- **Delete:** select a row or search first. The confirmation contains ID and name,
  with No selected by default. Yes removes that record, refreshes the grid and
  clears the form. No or a file error retains the record.
- **Summary:** Generate Summary Report rereads valid records from disk, displays
  a persistent read-only report, and overwrites `summary.txt`. It includes a local
  generation timestamp, totals, two-decimal averages and all five status counts.
  An empty file produces zero counts and N/A averages.

File errors appear in the form and retain input for retry. Updates and deletes
write a temporary file before replacing the original, preserving unrelated lines.

## Classification

Only `AnimalClassifier.Classify` assigns status and housing, including during update.
The user never types these two values.

| Recovery score | Status | Housing unit |
| --- | --- | --- |
| 0–19 | Critical | Intensive Care Unit |
| 20–39 | Serious | High-Dependency Ward |
| 40–59 | Stable | Recovery Ward |
| 60–79 | Recovering | Outdoor Enclosure |
| 80–100 | Release-Ready | Pre-Release Camp |

## Text file formats

`animals.txt` is UTF-8, one animal per line, seven pipe-separated fields, with no header:

```text
AnimalID|Name|Species|Age|RecoveryScore|Status|HousingUnit
```

The field-order line above is documentation, not a record. A real record is:

```text
WR-0002|Naledi|Serval|3|62|Recovering|Outdoor Enclosure
```

IDs must be `WR-` followed by exactly four ordinary digits. The starter file
contains eight unique animals across all five statuses. Add preserves old records.
Update and delete preserve the order of all remaining records. Loading recalculates
status and housing from the score, so stale stored derived values are corrected
in memory; saving an edited record writes the correct derived values.

`summary.txt` is UTF-8 text and is replaced each time a report is generated.
For the supplied eight records its layout is:

```text
BAOBAB RIDGE WILDLIFE REHABILITATION CENTRE
ANIMAL SUMMARY REPORT
Generated: 2026-10-20 18:00
----------------------------------------
Total animals:            8
Average age (years):      6.75
Average recovery score:   52.62
----------------------------------------
Animals per status:
  Critical:               2
  Serious:                1
  Stable:                 1
  Recovering:             1
  Release-Ready:          3
```

The generation time varies. Averages use a decimal point regardless of computer locale.
Generated `summary.txt`, `bin/`, `obj/`, `.vs/` and user settings are ignored by Git.

## Source structure

`MainForm.cs` connects Add, View, Summary and refresh events; `MainForm.Member3.cs`
contains the search/edit/delete handlers for the same form. `Animal.cs` stores
validated animal details. `Services/AnimalValidator.cs` checks user input,
`AnimalClassifier.cs` assigns the two derived values, `FileManager.cs` reads and
appends records, `UpdateDeleteService.cs` rewrites selected records, and
`SummaryReportService.cs` calculates and formats the report.

The application follows the supplied class examples: ordinary classes and properties,
separate form/file responsibilities, named event handlers, lists, loops,
`FileStream`, `StreamReader`, `StreamWriter`, `using`, and friendly `try/catch` handling.

## Tests

On Windows, run the dependency-free regression runner:

```powershell
dotnet run --project Tests/BaobabRidge.Tests.csproj -c Release
```

Tests use disposable files. They cover validation, every classification boundary,
duplicates after restart, UTF-8, missing files, malformed records, record order,
locked/read-only failures, shared edit mode, failed search preservation, cancellation,
all three grid refresh paths, exact summary format, empty data, and summary overwrite.

## Limitations and submission checks

- Use one running application instance. Simultaneous writes from multiple instances
  are not coordinated. Ambiguous duplicate IDs in hand-edited data cannot be edited
  until the file is corrected.
- Starter data is copied if newer. Back up runtime records before replacing a build
  or updating the starter file; a newer starter can replace an older runtime copy.
- The fixed layout needs a sufficiently large Windows desktop; smaller screens or
  high display scaling may require a larger display or lower scaling for presentation.
- Every member must verify their own real commits, dates, merged pull request and
  understanding of the code. An integration fix cannot supply another person's
  contribution history. Never backdate commits or attribute AI work to a student.
- Clone into a new empty folder and demonstrate all five features together before
  submitting the GitHub URL and clean project ZIP.

## Assistance disclosure

This project includes code, integration changes, tests and documentation prepared
with OpenAI Codex assistance. The supplied lecturer examples informed its structure.
This work must not be represented as entirely student-authored. The assignment's
academic-integrity rules still apply; the group must confirm permitted use with the
lecturer, review and understand the implementation, and describe contributions honestly.
