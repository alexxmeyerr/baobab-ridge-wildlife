# Member 1: Add Animal and validation

This implementation and its tests were prepared with AI assistance. Read and understand
each method before presenting or adapting it.

## Run your feature

Open `BaobabRidge.csproj` in Visual Studio and press F5. The initial form is the Add
Animal screen. A useful test entry is `wr-0101`, `Naledi`, `Serval`, `3`, `62`.
After Add, expect `WR-0101`, `Recovering`, and `Outdoor Enclosure` in the saved record.
The runtime file is under `bin/Debug/net8.0-windows/animals.txt` in a normal Debug build.

## What each file does

| File | Responsibility |
| --- | --- |
| `Animal.cs` | Holds the five validated inputs and two calculated outputs. ID is read-only. |
| `Services/AnimalValidator.cs` | Trims inputs, uppercases ID, validates each field, and creates an Animal. |
| `Services/AnimalClassifier.cs` | Calculates status and housing together in one method. |
| `Services/FileManager.cs` | Checks the current file for duplicate IDs and appends a seven-field UTF-8 line. |
| `MainForm.cs` | Handles Add/Clear, field focus, success messages, and friendly file errors. |
| `MainForm.Designer.cs` | Constructs the labelled inputs, buttons, and result labels. |
| `Tests/Program.cs` | Runs automated checks against temporary files and the real form handlers. |

## Presentation outline (about three minutes)

1. Explain the five inputs and that the user never enters status or housing.
2. Enter the example above with lowercase `wr-0101` and spaces around the name.
   Click Add. Show the uppercase ID, classification, confirmation, and cleared inputs.
3. Try adding the same ID again. Explain that the service reads the current file on
   every save, so duplicates are rejected even after restarting the program.
4. Change the ID and enter `62.5` as the score. Show the error and the cursor returning
   to Recovery Score. Explain that an invalid form does not write anything.
5. Open `AnimalClassifier.Classify`. Explain how the ordered comparisons cover all
   scores without gaps and set both outputs together.
6. Open `animals.txt` beside the executable and explain the seven fields. Add appends
   one line; it does not rewrite existing records.

## Classification to know for questions

| Score | Status | Housing Unit |
| --- | --- | --- |
| 0-19 | Critical | Intensive Care Unit |
| 20-39 | Serious | High-Dependency Ward |
| 40-59 | Stable | Recovery Ward |
| 60-79 | Recovering | Outdoor Enclosure |
| 80-100 | Release-Ready | Pre-Release Camp |

- Why reject `|`? It separates fields, so a pipe in a name would corrupt the format.
- Why reject embedded line breaks? Each animal must occupy exactly one line.
- Why `TryParse`? It reports invalid numeric text without throwing an exception.
- Why `AppContext.BaseDirectory`? It locates data beside the executable on any machine.
- Why trim first? Accidental surrounding spaces should not invalidate otherwise valid data.
- What happens at 79 and 80? Recovering changes to Release-Ready; the housing changes too.
- How does duplicate checking work? Read the current file, go through its lines,
  split each line at `|`, and compare the first field with the new ID.
- Why `FileMode.Append`? It adds to the file instead of replacing old records.
- What does `false` in `new UTF8Encoding(false)` mean? Do not write a UTF-8 byte-order
  mark. It does not control append mode.
- What does `out` mean? The method assigns a value to a variable supplied by its
  caller. `Classify` fills in both `status` and `housingUnit`.
- What happens after a file error? The inputs stay filled and the user can correct the
  file problem and retry; no success notification or refresh event is raised.

## Shared integration

The grid and summary are now integrated. `AnimalsChanged` reloads the grid once after
Add, Update or Delete. `AnimalAdded` is retained for compatibility, but the grid does
not also subscribe to it. The update workflow reuses your validator and classifier.
The summary uses the same file loader and counts the five existing statuses.

## Manual checks before merging

- Add a valid animal and verify its exact saved line.
- Restart and retry the same ID with lowercase letters; it must be rejected.
- Try blank fields, 31-character text, a pipe, decimal age/score, -1, and 101.
- Check both ends of every classification band, including 19/20 and 79/80.
- Make a disposable runtime animals.txt read-only; verify the friendly message and
  retained input, then restore write permission and retry.
- Hold a disposable animals.txt open with an exclusive lock; verify a retry message.
  A normal text editor may allow sharing and therefore may not cause a lock error.
- Confirm the file receives a separate new record even if its old last line has no newline.
- During integration, verify that the Member 2 grid refreshes once after a successful add.

## Read the simpler code in this order

1. `Animal.cs`: an ordinary class stores the animal's details. Its constructor calls
   the shared classifier and stores both results.
2. `AnimalClassifier.cs`: a single `if/else if/else` chain selects the score band.
3. `AnimalValidator.cs`: check the ID, name, species, age, and score one at a time.
   A failed check creates a `ValidationError` object and returns `false` immediately.
4. `FileManager.cs`: use `FileStream` and `StreamReader` to read lines into a
   `List<string>`. Scan IDs with `foreach`, then use `FileMode.Append` and
   `StreamWriter` to add one record.
5. `MainForm.cs`: the Add click handler validates, saves inside `try/catch`, then
   displays the result and clears the inputs. A normal `if/else` chain picks the
   input that needs focus after a validation error.

The file code is deliberately written for one running application instance. Two
copies could both check an ID before either saves it. The `Read` and append operations
are separate, following the lecture examples. It still handles locked and read-only
files. See `CLASS-EXAMPLES.md` for the source examples and the assignment-specific
checks retained in this rewrite.

## Git and presentation

The original Member 1 feature was reviewed and merged through pull requests.
Use the latest main branch for the complete application. Save local changes before
pulling. Never force-push an older local export over the shared repository.

Explain changes you understand and acknowledge the assistance disclosure above.
Commit real work on its actual dates; do not alter dates to simulate contributions.
