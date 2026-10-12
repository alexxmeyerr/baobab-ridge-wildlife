# Member 1: Add Animal and validation

This implementation and its tests were prepared with AI assistance. Read and understand
each method before presenting or adapting it.

## Run your feature

Open `BaobabRidge.csproj` in Visual Studio and press F5. The initial form is the Add
Animal screen. A useful test entry is `wr-0007`, `Naledi`, `Serval`, `3`, `62`.
After Add, expect `WR-0007`, `Recovering`, and `Outdoor Enclosure` in the saved record.
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
2. Enter the example above with lowercase `wr-0007` and spaces around the name.
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
- What does `true` in the StreamWriter constructor mean? Append to the file instead
  of replacing its contents.
- What does `out` mean? The method assigns a value to a variable supplied by its
  caller. `Classify` fills in both `status` and `housingUnit`.
- What happens after a file error? The inputs stay filled and the user can correct the
  file problem and retry; no success notification or refresh event is raised.

## Handoff to the group

Member 2: add the DataGridView and startup loader. Subscribe to `AnimalAdded` in the
form constructor after the grid is initialized, for example with a handler that calls
your `RefreshGrid` method. Refresh from disk after each successful Add. Keep loading
and skipped-line reporting in your own feature. Do not raise duplicate notifications
from both your event handler and the Add handler.

Member 3: load the same input boxes in edit mode, make the ID read-only, and call
`AnimalValidator.TryCreate` with the original ID. This builds a fresh Animal and calls
the same classifier even when the score is unchanged. Use your update/rewrite method,
not `TryAppend`, which intentionally rejects existing IDs. Keep Add disabled during
editing and restore Add mode after save or Clear. `Animal.AnimalId` cannot be changed.

Member 4: supply starter records in `Data/animals.txt`, implement your summary service,
and fill in the real team details. The project already copies `Data/animals.txt` beside
the executable with Copy if newer. Do not commit runtime `summary.txt`.

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
4. `FileManager.cs`: read the text, scan the IDs with a `while` loop, then use a
   `StreamWriter` to add one line to the end.
5. `MainForm.cs`: the Add click handler validates, saves inside `try/catch`, then
   displays the result and clears the inputs. A normal `if/else` chain picks the
   input that needs focus after a validation error.

The file code is deliberately written for one running application instance. Two
copies could both check an ID before either saves it. The earlier exclusive-stream
implementation handled that extra scenario; this version uses the simpler file
methods from the lecture material. It still handles locked and read-only files.

## Git handoff

Member 1 work is published on `feature/add-animal` for review through a pull request.
Clone the GitHub repository and switch to that branch to work on the shared version.
The earlier local export has a separate initial Git history; do not force-push it
over the shared repository.

Commit real changes on the days you work. This implementation does not satisfy the
three-distinct-days requirement by itself; do not change commit dates to simulate work.
Use a pull request and teammate review before merging the feature into `main`.
