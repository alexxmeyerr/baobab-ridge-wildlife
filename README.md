# Baobab Ridge Wildlife Rehabilitation Records System

C# Windows Forms project for PRG2782, targeting .NET 8.

## Team

Fill in all member names and student numbers before submission.

| Member | Name | Student number | Feature |
| --- | --- | --- | --- |
| 1 | To complete | To complete | Add Animal and validation |
| 2 | To complete | To complete | View Animals |
| 3 | To complete | To complete | Update and Delete |
| 4 | To complete | To complete | Summary and GitHub management |

## Build and run

Open `BaobabRidge.csproj` in Visual Studio with the .NET desktop development workload and .NET 8 support. Press F5.

Alternatively, on Windows with a .NET 8 or newer SDK:

```powershell
dotnet build BaobabRidge.csproj
dotnet run --project BaobabRidge.csproj
```

## Member 1 feature

The `feature/add-animal` branch implements Add Animal. Enter Animal ID, Name,
Species, Age, and Recovery Score, then click **Add Animal**. A successful save
clears the inputs and displays the animal's calculated status and housing unit.
An invalid entry names the field and focuses it. File errors leave your input
available for a retry.

- IDs are trimmed, uppercased, formatted as `WR-0007`, and unique in the current file.
- Names and species are trimmed, 1-30 characters, and cannot contain pipes or line breaks.
- Age and score accept decimal digits representing whole numbers from 0 to 100.
- `AnimalClassifier.Classify` is the one method that calculates both derived fields.
- Add appends a UTF-8 record while preserving all existing bytes.
- `MainForm.AnimalAdded` lets Member 2 refresh the grid after successful saves.

## Files and locations

`animals.txt` lives beside the running executable, regardless of the working directory.
`Data/animals.txt` is currently empty and uses **Copy if newer**, with its output
name set to `animals.txt`. Member 4 can replace the source copy with the required
eight or more starter records before final integration. A missing runtime file
is created when the first valid animal is added.

Each record has seven pipe-separated fields, no header, and no blank lines:

```text
WR-0007|Naledi|Serval|3|62|Recovering|Outdoor Enclosure
```

The field order is ID, Name, Species, Age, Score, Status, Housing Unit.
Member 4's `summary.txt` will be a generated text report beside the executable,
overwritten each time and excluded from Git. Summary generation is not implemented
on this branch.

## Tests

Run on Windows with the .NET desktop SDK/runtime:

```powershell
dotnet run --project Tests/BaobabRidge.Tests.csproj
```

The test runner requires no external packages. It checks field validation,
classification boundaries, file preservation, duplicate IDs across instances,
UTF-8 names, missing/locked/read-only files, and the real form button handlers.
It briefly opens a test form and uses temporary data rather than your saved records.

## Integration and limitations

Add and Member 3's Update/Delete now share `MainForm`, the `Animal` model,
validation, classification, and one runtime file. View, Summary, and starter
records remain to be integrated. No DataGridView is included yet. See
`INTEGRATION.md` for the combined workflow and Member 2's refresh connection.
The UI layout is constructed in code in `MainForm.Designer.cs`.

The uniqueness scan reserves IDs even in malformed existing lines. Member 2 should
handle malformed loading and report the skipped-line count. This simpler version
is designed for one running instance of the application: checking IDs and appending
are separate steps, so do not run two copies against the same file at once.

## Straightforward C# style

The application uses private backing fields and properties like the lecturer's
`Student` class, ordinary constructors, brace-based namespaces, explicit variable
types, loops, `if/else` statements, and traditional `using` blocks. The ID check
uses a loop instead of a regular expression. Classification sets two `out` string
parameters instead of returning a tuple. File handling follows the lecturer's examples: `FileStream`, `StreamReader`,
`List<string>`, a `while` loop to read lines, a `foreach` loop to check IDs, and
`StreamWriter` with `FileMode.Append`. Streams are enclosed in `using` blocks.

The form layout uses normal control properties such as `Location`, `Size`, and
`Text`, with named click handlers. `Tests/` is a separate verification program;
start with the application files when preparing your presentation.
See `CLASS-EXAMPLES.md` for the exact examples used and how they map to this project.

Rebuilding may refresh the runtime data if `Data/animals.txt` becomes newer. Back up
runtime records before intentionally changing the starter file.

Member names, student numbers, three-day contribution history, and merged feature
pull requests must still be completed by the group. See `MEMBER1.md` for a manual
demo and testing checklist.
